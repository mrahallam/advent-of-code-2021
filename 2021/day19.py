import numpy as np
import re
from copy import deepcopy

data = 'test19.txt'

with open(data) as file:
    for line in file:
        data += line

scanners = []

for scanner in re.split(r'\n\n', data):
    lines = scanner.split('\n')
    n = int(re.search(r'\d+', lines[0][lines[0].find('scanner'):]).group())
    #scanners[n] = [np.array([[x for x in row] for row in lines[1:]],dtype=object)]
    scanners.append(np.array([[int(x) for x in row.strip().split(',')] for row in lines[1:]]))

def get_rotations(np_array):
    rotations = []
    rotations.append(np_array)
    temp = np.copy(np_array)
    temp = temp[:,[0, 2, 1]]
    rotations.append(temp)
    temp = temp[:,[1, 0, 2]]
    rotations.append(temp)
    temp = temp[:,[0, 2, 1]]
    rotations.append(temp)
    temp = temp[:,[1, 0, 2]]
    rotations.append(temp)
    temp = temp[:,[0, 2, 1]]
    rotations.append(temp)

    invert_0 = deepcopy(rotations)
    invert_1 = deepcopy(rotations)
    invert_2 = deepcopy(rotations)

    for rotation in invert_0:
        rotation[:,0] *= -1
        
    for rotation in invert_1:
        rotation[:,1] *= -1
        
    for rotation in invert_2:
        rotation[:,2] *= -1

    rotations = rotations+invert_0+invert_1+invert_2
    return rotations

def get_distances(np_array):
    distances = []
    for i, i_val in enumerate(np_array):
        for j, j_val in enumerate(np_array):
            if j != i:
                x_dist = i_val[0] - j_val[0]
                y_dist = i_val[1] - j_val[1]
                z_dist = i_val[2] - j_val[2]
                difference = (x_dist,y_dist,z_dist)
                distances.append((difference,(i_val,j_val)))
    return distances

def get_inverse(a_tuple):
    return_list = []
    for i in a_tuple:
        return_list.append(i*-1)
    return tuple(return_list)

def get_coordinates_matching_beacons(scanner1, scanner2):
    distances_0 = get_distances(scanner1)
    rotations_1 = get_rotations(scanner2)
    for ir1, r1 in enumerate(rotations_1):
        reduced_list = []
        matching_beacons = []
        distances_1 = get_distances(r1)
        counter = 0
        for i in distances_0:
            for j in distances_1:
                if i[0] == j[0]:
                    if (tuple(i[1][0]) not in [item[1] for item in matching_beacons]
                        and tuple(i[1][1]) not in [item[2] for item in matching_beacons]):
                        matching_beacons.append((ir1,tuple(i[1][0]),tuple(i[1][1]),tuple(j[1][0]),tuple(j[1][1])))

        matching_beacons = list(dict.fromkeys(matching_beacons))
        for j in matching_beacons:
            if j not in reduced_list:
                reduced_list.append(j)
            
        if(len(reduced_list) > 11):
            return reduced_list,r1


def get_scanner_translation(scanner1,scanner2):
    matching_beacons,the_grid = get_coordinates_matching_beacons(scanner1,scanner2)
    xyz_differences = []
    if matching_beacons:
        for i in matching_beacons:
            xyz_differences.append((i[1][0] + i[3][0],i[1][1] + i[3][1],i[1][2] + i[3][2]))
            xyz_differences.append((i[1][0] + i[4][0],i[1][1] + i[4][1],i[1][2] + i[4][2]))
            xyz_differences.append((i[1][0] - i[3][0],i[1][1] - i[3][1],i[1][2] - i[3][2]))
            xyz_differences.append((i[1][0] - i[4][0],i[1][1] - i[4][1],i[1][2] - i[4][2]))
        return max(set(xyz_differences), key=xyz_differences.count), the_grid


new_translation,the_grid = get_scanner_translation(scanners[0],scanners[1])
print(new_translation)

the_grid[:,0] = new_translation[0] - the_grid[:,0]
the_grid[:,1] = new_translation[1] - the_grid[:,1]
the_grid[:,2] = new_translation[2] - the_grid[:,2]
print(the_grid)

new_translation, the_grid = get_scanner_translation(the_grid,scanners[3])
print(new_translation)

the_grid[:,0] = new_translation[0] - the_grid[:,0]
the_grid[:,1] = new_translation[1] - the_grid[:,1]
the_grid[:,2] = new_translation[2] - the_grid[:,2]
print(the_grid)

#new_translation, the_grid = get_scanner_translation(the_grid,scanners[4])
#print(new_translation)

#the_grid[:,0] = new_translation[0] - the_grid[:,0]
#the_grid[:,1] = new_translation[1] - the_grid[:,1]
#the_grid[:,2] = new_translation[2] - the_grid[:,2]
#print(the_grid)

new_translation, the_grid = get_scanner_translation(the_grid,scanners[2])
print(new_translation)

the_grid[:,0] = new_translation[0] + the_grid[:,0]
the_grid[:,1] = new_translation[1] + the_grid[:,1]
the_grid[:,2] = new_translation[2] + the_grid[:,2]
print(the_grid)
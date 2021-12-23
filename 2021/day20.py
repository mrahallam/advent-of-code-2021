file_name = 'input20.txt'
#file_name = 'test20.txt'
image = []
infinite_space = '0'
n = 50 #how many loops
from copy import deepcopy

with open(file_name) as f:
    image_enhancement_algorithm = [x for x in f.readline().rstrip()]
    f.readline()
    for line in f:
        image.append([x for x in line.rstrip()])

for i,v in enumerate(image_enhancement_algorithm[:]):
    if v == '.':
        image_enhancement_algorithm[i] = '0'
    else:
        image_enhancement_algorithm[i] = '1'

for i,v in enumerate(image[:]):
    for j,w in enumerate(v):
        if w == '.':
            image[i][j] = '0'
        else:
            image[i][j] = '1'

def expand_image(array):
    global infinite_space
    new_list = []
    for i in range(len(array[0])):
        new_list.append(infinite_space)

    for i in range(3):
        array.insert(0,new_list)
        array.append(new_list)

    new_row = [infinite_space,infinite_space,infinite_space]
    new_image = []    
    for row in array:
        new_image.append(new_row + row + new_row)

    return new_image

def get_binary(array,y,x):
    global infinite_space
    binary = ''
    for i in range (-1,2,1):
        for j in range(-1,2,1):
            if y >= 0 and x >= 0 and y < len(array)-1 and x < len(array[0])-1:
                binary += array[i+y][j+x]
            else:
                binary += infinite_space
    return binary

def enhance_image(array):
    global infinite_space
    array = expand_image(array)
    before_enhancement = deepcopy(array)
    for i,v in enumerate(before_enhancement):
        for j, w in enumerate(v):
            binary_number = get_binary(before_enhancement,i,j)
            loc = int(binary_number,2)
            array[i][j] = (image_enhancement_algorithm[loc])

    infinite_space_string = ''
    for i in range(9):
        infinite_space_string += infinite_space
    infinite_space = (image_enhancement_algorithm[int(infinite_space_string,2)])
    
    return array

for i in range(n):
    if i == 0:
        result = enhance_image(image)
    else:
        result = enhance_image(result)
# print each iteration if you want!
#    for i in result:
#        print(''.join(i))

pixel_count = 0
for i in result:
    for j in i:
        if j == '1':
            pixel_count += 1

print(pixel_count)

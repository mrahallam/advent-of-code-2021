#file_name = 'test13.txt'
file_name = 'input13.txt'
import re
from copy import deepcopy
import numpy

with open(file_name, 'r') as file:
    data, data1 = [x.splitlines() for x in file.read().split('\n\n')]
    coordinates = set([tuple(int(z) for z in y.split(',')) for y in data])

    width = max([x[0] for x in coordinates])
    height = max([y[1] for y in coordinates])

    print(width,height)

    original_grid = []

    for y in range(height+1):
        grid_row = []
        for x in range(width+1):
            if (x,y) in coordinates:
                grid_row.append('#')
            else:
                grid_row.append('.')
        original_grid.append(grid_row)
    
#    for i in original_grid:
#        print(i)

    folds = []

    for index, instruction in enumerate(data1):
        m = re.search(r'\d+$', instruction)
        fold = (int(m.group()) if m else None)
        if 'x' in instruction:
            folds.append([index,'x',fold])
        elif 'y' in instruction:
            folds.append([index,'y',fold])

    for i in folds:
        print(i)

    #folds = [fold for fold in folds if fold[0] == 0] #just first instruction for part 1

    def flip_horizontal(grid,value):
        horizontally_folded_grid = []
        if value >= width/2:
            number_to_flip = (width - value)
            for i in range (1,number_to_flip+1):
                for row in grid:
                    if row[value-i] != '#' and row[value+i] == '#':
                        row[value-i] = '#'
            for i in grid:
                horizontally_folded_grid.append(i[:value])

        elif value < width/2:
            #print('doing this one')
            number_to_flip = (value)
            for i in range (1,number_to_flip+1):
                for row in grid:
                    #print(value,i,row[value-1-i],row[value-1+i],"".join(row))
                    if row[value-i] == '#' and row[value+i] != '#':
                        row[value+i] = '#'
            for i in grid:
                horizontally_folded_grid.append(i[value+1:])

        return horizontally_folded_grid

        
    def flip_vertical(grid,value):
        vertically_folded_grid = []
        if value >= height/2:
            number_to_flip = (height - value)
            for i in range(1,number_to_flip+1):#1-4
                for index, coordinate in enumerate(grid[value + i]):
                    mirror_coordinate = grid[value - i][index]
                    if coordinate == '#' and mirror_coordinate != '#':
                        grid[value - i][index] = '#'
            for i in grid[:value]:
                vertically_folded_grid.append(i)

        elif value < height/2:
            print('doing this one')
            number_to_flip = (value)
            for i in range(1,number_to_flip+1):#1-4
                for index, coordinate in enumerate(grid[value - i]):
                    mirror_coordinate = grid[value + i][index]
                    if coordinate == '#' and mirror_coordinate != '#':
                        grid[value + i][index] = '#'
            for i in grid[value+1:]:
                vertically_folded_grid.append(i)

        return vertically_folded_grid

    dot_count = 0
    for i in original_grid:
        print("".join(i))
        dot_count += i.count('#')
    print(dot_count)   

    for i in folds:
        if i[1] == 'y':
            print('flipping vertically')
            original_grid = flip_vertical(original_grid,i[2])
            dot_count = 0
            for i in original_grid:
                print("".join(i))
                dot_count += i.count('#')
            print(dot_count)    

        if i[1] == 'x':
            print('flipping horizontally')
            original_grid = flip_horizontal(original_grid,i[2])
            dot_count = 0
            for i in original_grid:
                print("".join(i))
                dot_count += i.count('#')
            print(dot_count)
        
        arr = numpy.array(original_grid)
        arr = numpy.rot90(arr,2)
        rotated_list = arr.tolist()
 
        for i in rotated_list:
            print("".join(i))

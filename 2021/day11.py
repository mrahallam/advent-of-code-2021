#file_name = 'test11.txt'
file_name = 'input11.txt'

import copy

with open(file_name) as file:
    lines = file.readlines()
    lines = [list((line.strip())) for line in lines]

for i, line in enumerate(lines[:]):
    lines[i] = [int(i) for i in line]

pt2 = copy.deepcopy(lines)

def step_1(array):
    for i, line in enumerate(array):
        for j, octopus in enumerate(array):
            array[i][j] += 1

height = len(lines)
width = len(lines[0])
steps = 100
flash_count = 0
flashed = []

for i in range(height):
    flashed.append([False] * width)

def increase_surrounding_one(array,x,y):
    #top left
    if x > 0 and y > 0:
        array[y-1][x-1] += 1
        if array[y-1][x-1] > 9 and not flashed[y-1][x-1]:
            flash(array)
    #left
    if x > 0:
        array[y][x-1] += 1
        if array[y][x-1] > 9 and not flashed[y][x-1]:
            flash(array)
    #bottom left
    if x > 0 and y < height - 1:
        array[y+1][x-1] += 1
        if array[y+1][x-1] > 9 and not flashed[y+1][x-1]:
            flash(array)
    #top
    if y > 0:
        array[y-1][x] += 1
        if array[y-1][x] > 9 and not flashed[y-1][x]:
            flash(array)
    #bottom
    if y < height - 1: 
        array[y+1][x] += 1
        if array[y+1][x] > 9 and not flashed[y+1][x]:
            flash(array)
    #top right
    if x < width - 1 and y > 0:
        array[y-1][x+1] += 1
        if array[y-1][x+1] > 0 and not flashed[y-1][x+1]:
            flash(array)
    #right
    if x < width - 1:
        array[y][x+1] += 1
        if array[y][x+1] > 9 and not flashed[y][x+1]:
            flash(array)
    #bottom right
    if x < width - 1 and y < height - 1:
        array[y+1][x+1] += 1
        if array[y+1][x+1] > 9 and not flashed[y+1][x+1]:
            flash(array)

def flash(array):
    global flash_count
    for y, line in enumerate(array):
        for x, octopus in enumerate(line):
            if octopus > 9 and not flashed[y][x]:
                flashed[y][x] = True
                flash_count += 1
                increase_surrounding_one(array,x,y)

def step_3(array):
    for y, line in enumerate(array):
        for x, octopus in enumerate(line):
            if octopus > 9:
                flashed[y][x] = False
                array[y][x] = 0

for i in range(steps):
    step_1(lines)
    flash(lines)
    step_3(lines)

print(f'after {steps} steps, {flash_count} flashes')

step_counter = 0
while True:
    step_1(pt2)
    flash(pt2)
    step_3(pt2)
    step_counter += 1
    array_sum = 0
    for i in pt2:
        for j in i:
            array_sum += j

    if array_sum == 0:
        print(f'step where all octopus flash is {step_counter}')
        break

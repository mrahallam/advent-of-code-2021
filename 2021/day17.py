file_name = 'test17.txt'
file_name = 'input17.txt'

with open(file_name) as file:
    line = file.readline()

import re
import csv

numbers = re.findall('[+-]?\d+(?:\.\d+)?', line)

x1 = int(numbers[0])
x2 = int(numbers[1])
y1 = int(numbers[2])
y2 = int(numbers[3])
valid_velocities = {}

if y1 > y2:
    y_min, y_max = y2,y1
else:
    y_min, y_max = y1,y2 

if x1 > x2:
    x_min, x_max = x2,x1
else:
    x_min, x_max = x1,x2 


def fire_projectile(start_pos,initial_x_velocity,initial_y_velocity):
    global x_min,x_max,y_min,y_max 
    global valid_velocities 
    x_velocity = initial_x_velocity
    y_velocity = initial_y_velocity
    max_height = None
    while start_pos[0] <= x_max and start_pos[1] >= y_min:
        start_pos[0] += x_velocity
        start_pos[1] += y_velocity
        if x_velocity > 0:
            x_velocity -= 1
        elif x_velocity < 0:
            x_velocity += 1
        y_velocity -= 1
        if (x_min <= start_pos[0] <= x_max and y_min <= start_pos[1] <= y_max):
            if max_height == None:
                max_height = start_pos[1]
            elif start_pos[1] > max_height:
                max_height = start_pos[1]
            if (initial_x_velocity,initial_y_velocity) not in valid_velocities:
                valid_velocities[(initial_x_velocity,initial_y_velocity)]=max_height
    
    return valid_velocities

fire_projectile([0,0],30,-10)

for i in range(-x_max,x_max+1):
    for j in range(-(x_max//2),(x_max//2)+1):
        fire_projectile([0,0],i,j)

print(f'max height: {max(valid_velocities.values())}')
print(f'number of distinct initual velocity values: {len(valid_velocities)}')

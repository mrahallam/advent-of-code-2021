import math

test_input = [16,1,2,0,4,2,7,1,2,14]
total_fuel_used_pt1 = 0
total_fuel_used_pt2 = 0

input22 = open('input7.txt', 'r')
from statistics import median,mean
for line in input22:
    horizontal_positions = [int(x) for x in line.split(',')]
    #horizontal_positions = test_input
    median_position = int(median(horizontal_positions))
    mean_lower = int(math.floor(mean(horizontal_positions)))
    mean_upper = int(math.ceil(mean(horizontal_positions)))

for i in horizontal_positions:
    fuel_used = abs(i-median_position)
    total_fuel_used_pt1 += fuel_used

for i in range(mean_lower,mean_upper+1):
    total_fuel_used = 0
    for j in horizontal_positions:
        fuel_used = abs(j-i)
        #Gauss
        #series_sum = (fuel_used*(fuel_used+1))/2
        series_sum =  sum(range(0,fuel_used + 1))
        total_fuel_used += series_sum
    if total_fuel_used_pt2 == 0:
        total_fuel_used_pt2 = total_fuel_used
    elif total_fuel_used < total_fuel_used_pt2:
        total_fuel_used_pt2 = total_fuel_used

print(f'part 1: {total_fuel_used_pt1}\npart 2: {total_fuel_used_pt2}')

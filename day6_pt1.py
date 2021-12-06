import re

start_fish = []

input22 = open('input6.txt', 'r')
for line in input22:
    start_fish = line.strip().split(',') + start_fish

for i,v in enumerate(start_fish):
    start_fish[i] = int(v)

for i in range(80):
    for j,v in enumerate(start_fish[:]):
        if v == 0:
            start_fish.append(8)
            start_fish[j] = 6
        else:
            start_fish[j] -= 1

print(len(start_fish))

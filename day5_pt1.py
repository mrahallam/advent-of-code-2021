import re
with open('input5.txt') as file:
    lines = file.readlines()
    lines = [line.strip().split('->') for line in lines]
    lines = [[i.strip().split(',') for i in line] for line in lines]
    horizontal_or_vertical = []

points_covered = []

for line in lines:
    for i,v in enumerate(line[:]):
        line[i] = ([int(i) for i in v])

for line in lines:
    if line[0][0] == line[1][0] or line[0][1] == line[1][1]:
        horizontal_or_vertical.append(line)

for line in horizontal_or_vertical:
    if line[0][0] == line[1][0]:
        if line[0][1] > line[1][1]:
            for i in range (line[1][1],line[0][1] + 1):
                points_covered.append([line[0][0],i])
        elif line[0][1] < line[1][1]:
            for i in range (line[0][1],line[1][1] + 1):
                points_covered.append([line[0][0],i])
    elif line[0][1] == line[1][1]:
        if line[0][0] > line[1][0]:
            for i in range(line[1][0],line[0][0] + 1):
                points_covered.append([i,line[0][1]])
        elif line[0][0] < line[1][0]:
            for i in range(line[0][0],line[1][0] + 1):
                points_covered.append([i,line[0][1]])

new_list = []
for i in points_covered:
    new_list.append(str(i[0])+', '+str(i[1]))

from collections import Counter
cat = Counter(new_list)
dog = Counter({k: c for k, c in cat.items() if c > 1})
print(len(dog))

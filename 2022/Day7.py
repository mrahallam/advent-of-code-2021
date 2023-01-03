from collections import defaultdict
from multiprocessing import Value
from pathlib import Path

path = []
folders = defaultdict(int)

with open('TestData7.txt', 'r') as f:
    for line in f:
        if line[:7] == '$ cd ..':
            path.pop()
        elif (line[:4] == '$ cd' or line == '﻿$cd /'):
            path.append(line.strip()[5:])
        elif line[0].isdigit():
            size, _ = line.split()
            for i in range(len(path)):
                print(",".join(path) + " " + size)
                folders['/'.join(path[:i + 1])] += int(size)
                #print('/'.join(path[:i + 1]) + ' is the key, ' + size + " is the value")
for k,v in folders.items():
    print(k + " ==> " + str(v))
print('Part 1:', sum(f for f in folders.values() if f < 100_000))
#print('Part 2:', min([f for f in folders.values() if folders['/'] - f <= 40_000_000]))
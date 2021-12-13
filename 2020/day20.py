import numpy as np
import re

#data = 'test7.txt'
data = 'input20.txt'

with open(data) as file:
    for line in file:
        data += line

tiles = {}
unflipped_tiles = {}

for tile in re.split(r'\n\n', data):
    lines = tile.split('\n')
    n = int(lines[0].split()[1][:-1])
    tiles[n] = [np.array([[x for x in row] for row in lines[1:]])]

for tile in re.split(r'\n\n', data):
    lines = tile.split('\n')
    n = int(lines[0].split()[1][:-1])
    unflipped_tiles[n] = [np.array([[x for x in row] for row in lines[1:]])]

for k, v in tiles.items():
    for i in range(3):
        tiles[k].append(np.rot90(tiles[k][i]))
    tile = np.flip(tiles[k][0], 0)
    tiles[k].append(tile)
    for i in range(3):
        tiles[k].append(np.rot90(tiles[k][4+i]))

for k, v in unflipped_tiles.items():
    for i in range(3):
        unflipped_tiles[k].append(np.rot90(unflipped_tiles[k][i]))

edges = []
for k, v in tiles.items():
    for i in v:
        edges.append([k,list(i[:,0])])
        edges.append([k,list(i[:,9])])
        edges.append([k,list(i[0])])
        edges.append([k,list(i[9])])

unique_edges = []
[unique_edges.append(x) for x in edges if x not in unique_edges]

just_the_edges = []
for i in unique_edges:
    just_the_edges.append(str(i[1]))

from collections import Counter
count_edges = Counter(just_the_edges)
filtered_count_edges = Counter({k: c for k, c in count_edges.items() if c == 1})

edge_tiles = []

for k, v in filtered_count_edges.items():
    #print(k, '-->', v)
    for i in unique_edges:
        if str(i[1]) == str(k):
            edge_tiles.append(i[0])

count_edge_tiles = Counter(edge_tiles)
filtered_count_edge_tiles = Counter({k: c for k, c in count_edge_tiles.items() if c == 4})

multiplied_ids = 1

for k, v in filtered_count_edge_tiles.items():
    multiplied_ids *= k

print(multiplied_ids)
#29584525501199

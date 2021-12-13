from collections import defaultdict

#file_name = 'test12.txt'
file_name = 'input12.txt'
#file_name = 'test12_larger.txt'
#file_name = 'test12_even_larger.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [tuple((line.strip().split('-'))) for line in lines]

cave_links = defaultdict(set)

for a,b in lines:
    cave_links[a].add(b)
    cave_links[b].add(a)

# Find all paths
def find_all_paths(graph, start, end, path=[]):
        path = path + [start]
        if start == end:
            return [path]
        if start not in graph:
            return []
        paths = []
        for node in graph[start]:
            other_small_two_visits = False
            for i in path:
                if i.islower() and i!='start' and i!='end' and i!=node:
                    counter = path.count(i)
                    if counter > 1:
                        other_small_two_visits = True

            count = path.count(node)
            if node not in path or not node.islower() or (not other_small_two_visits and count <2 and node.islower and node != 'start' and node != 'end'):
                newpaths = find_all_paths(graph, node, end, path)
                for newpath in newpaths:
                    paths.append(newpath)
        return paths  

paths=find_all_paths(cave_links,'start','end')

print(len(paths))

reduced_paths = []

for path in paths:
    small_caves = {}
    for k,v in cave_links.items():
        if k.islower() and k != 'start' and k != 'end':
            small_caves[k] = 0

    for cave in path:
        if cave in small_caves:
            small_caves[cave] += 1
    
    counter = 0
    for k, v in small_caves.items():
        if v > 1:
            counter += 1
    
    if counter < 2:
        reduced_paths.append(path)
        
print(len(reduced_paths))

from collections import defaultdict

#file_name = 'test12.txt'
file_name = 'input12.txt'
#file_name = 'test12_larger.txt'
#file_name = 'test12_even_larger.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [tuple((line.strip().split('-'))) for line in lines]

cave_links = defaultdict(set)
n = 1 #number of times a node can be visited

for a,b in lines:
    cave_links[a].add(b)
    cave_links[b].add(a)

def find_all_paths(n, graph, start, end, path=[]):
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
        if node not in path or not node.islower() or (not other_small_two_visits and count < n and node.islower and node != 'start' and node != 'end'):
            newpaths = find_all_paths(n, graph, node, end, path)
            for newpath in newpaths:
                paths.append(newpath)
    return paths  

while n < 3:
    paths=find_all_paths(n, cave_links,'start','end')
    print(f'part {n}: {len(paths)}')
    n += 1
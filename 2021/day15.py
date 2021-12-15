#file_name = 'test/test15.txt'
file_name = 'input/input15.txt'
grid_size = 5
import numpy as np

with open(file_name) as file:
    lines = file.readlines()
    lines = [list((line.strip())) for line in lines]

for i, line in enumerate(lines[:]):
    lines[i] = [int(i) for i in line]

original_width = len(lines[0])
original_height = len(lines)

arr = np.asarray(lines)

def increment_array(array):
    incremented_array = np.where(array < 9, array + 1, 1)
    return incremented_array

for i in range(grid_size):
    if i == 0:
        row_arr = arr
    else:
        arr = increment_array(arr)
        row_arr = np.concatenate((row_arr,arr),axis=1)

for i in range(grid_size):
    if i == 0:
        col_arr = row_arr
    else:
        row_arr = increment_array(row_arr)
        col_arr = np.concatenate((col_arr,row_arr),axis=0)

# is there any benefit to keeping it as an np array?
lines = col_arr.tolist()

map_width = len(lines[0]) - 1  
map_height = len(lines) - 1
graph = {}
costs = {}
parents = {}

start_node = (0,0)
end_node = (map_width,map_height)

#reduce the number of nodes
def out_of_bound(x,y):
    if not (x > (3 * original_width)-1 and y <= (2 * original_height)-1 
            or (x <= (2 * original_width)-1 and y > (3 * original_height)-1)
            #not sure if these reductions are valid! works for test
            or (x <= original_width-1 and y > (2 * original_height)-1)
            or (x <= (3 * original_width)-1 and y > (4 * original_height)-1)
            or (x > (2 * original_width)-1 and y <= original_height-1)
            or (x > (4 * original_width)-1 and y <= (3 * original_height)-1)):
        return False
    else:
        return True

def get_left(x,y):
    if x > 0 and not out_of_bound(x-1,y):
        return([(x-1,y),lines[y][x-1]])

def get_right(x,y):
    if x < map_width and not out_of_bound(x+1,y):
        return([(x+1,y),lines[y][x+1]])
    
def get_up(x,y):
    if y > 0 and not out_of_bound(x,y-1):
        return([(x,y-1),lines[y-1][x]])

def get_down(x,y):
    if y < map_height and not out_of_bound(x,y+1):
        return([(x,y+1),lines[y+1][x]])

def get_surrounding(x,y):
    surrounding = {}
    if get_left(x,y):
        if not (x,y) in surrounding:
            surrounding[get_left(x,y)[0]]=get_left(x,y)[1]
        else:
            surrounding[get_left(x,y)[0]]+=get_left(x,y)[1]
    if get_right(x,y):
        if not (x,y) in surrounding:
            surrounding[get_right(x,y)[0]]=get_right(x,y)[1]
        else:
            surrounding[get_right(x,y)[0]]+=get_right(x,y)[1]
    if get_up(x,y):
        if not (x,y) in surrounding:
            surrounding[get_up(x,y)[0]]=get_up(x,y)[1]
        else:
            surrounding[get_up(x,y)[0]]+=get_up(x,y)[1]
    if get_down(x,y):
        if not (x,y) in surrounding:
            surrounding[get_down(x,y)[0]]=get_down(x,y)[1]
        else:
            surrounding[get_down(x,y)[0]]+=get_down(x,y)[1]
    return surrounding

for i, value_i in enumerate(lines):
    for j, value_j in enumerate(value_i):
        if not out_of_bound(j,i):
            if not (j,i) in graph:
                graph[(j,i)] = get_surrounding(j,i)
            else:
                graph[(j,i)] += get_surrounding(j,i)

#print(len(graph))

for k in graph:
    if k == (0,0):
        costs[k]=0
    else:
       costs[k] = np.inf

def search(source, target, graph, costs, parents):
    nextNode = source
    while nextNode != target:
        for neighbor in graph[nextNode]:
            if graph[nextNode][neighbor] + costs[nextNode] < costs[neighbor]:
                costs[neighbor] = graph[nextNode][neighbor] + costs[nextNode]
                parents[neighbor] = nextNode
            del graph[neighbor][nextNode]
        del costs[nextNode]
        nextNode = min(costs, key=costs.get)
    return parents

result = search(start_node, end_node, graph, costs, parents)

def backpedal(source, target, searchResult):
    node = target
    backpath = [target]
    path = []
    while node != source:
        backpath.append(searchResult[node])
        node = searchResult[node]
    for i in range(len(backpath)):
        path.append(backpath[-i - 1])
    return path

#print('parent dictionary={}'.format(result))
#print('longest path={}'.format(backpedal(start_node, end_node, result)))
result_path = backpedal(start_node, end_node, result)

cost = 0

for i in result_path[1:]:
    cost += lines[i[1]][i[0]]

print(cost)

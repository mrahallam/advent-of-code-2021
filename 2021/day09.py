#file_name = 'test9.txt'
file_name = 'input9.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [list((line.strip())) for line in lines]

for i, line in enumerate(lines[:]):
    lines[i] = [int(i) for i in line]

map_width = len(lines[0]) - 1  
map_height = len(lines) - 1
low_points = []
basins = []
risk = 0
#top n basins
n = 3

def get_risk(height):
    return height + 1

def get_left(x,y):
    if x > 0:
        return([(x-1,y),lines[y][x-1]])

def get_right(x,y):
    if x < map_width:
        return([(x+1,y),lines[y][x+1]])
    
def get_up(x,y):
    if y > 0:
        return([(x,y-1),lines[y-1][x]])

def get_down(x,y):
    if y < map_height:
        return([(x,y+1),lines[y+1][x]])

def get_surrounding(x,y):
    surrounding = []
    if get_left(x,y):
        surrounding.append(get_left(x,y))
    if get_right(x,y):
        surrounding.append(get_right(x,y))
    if get_up(x,y):
        surrounding.append(get_up(x,y))
    if get_down(x,y):
        surrounding.append(get_down(x,y))
    return surrounding

def is_low(x,y):
    dir_count = 0
    true_count = 0
    spot_height = lines[y][x]
    right = get_right(x,y)
    left = get_left(x,y)
    up = get_up(x,y)
    down = get_down(x,y)

    if right:
        dir_count +=1
        if spot_height < right[1]:
            true_count += 1
    if left:
        dir_count +=1
        if spot_height < left[1]:
            true_count += 1
    if up:
        dir_count +=1
        if spot_height < up[1]:
            true_count += 1
    if down:
        dir_count +=1
        if spot_height < down[1]:
            true_count += 1

    return true_count == dir_count

#get low points
for y, line in enumerate(lines):
    for x, spot_height in enumerate(line):
        if is_low(x,y):
            low_points.append([(x,y),lines[y][x]])

def get_risk(height):
    return height + 1

for y, line in enumerate(lines):
    for x, spot_height in enumerate(line):
        if is_low(x,y):
            risk += get_risk(spot_height)
            
print(f'Risk is {risk}')

#get basins
for i in low_points:
    already_checked = [i]
    while True:

        outer_points = []
        for j in already_checked:
            surrounding_j = get_surrounding(j[0][0],j[0][1])
            for k in surrounding_j:
                if  k not in already_checked and k[1] < 9:
                    outer_points.append(k)
        if len(outer_points) == 0:
            break
        for j in outer_points:
            if j not in already_checked:
                already_checked.append(j)
    basins.append(len(already_checked))

#calculate product of top n basin sizes
basin_size_product = 1
basins.sort()
basins = basins[-n:]
for i in basins:
    basin_size_product *= i

print(f'Product of top {n} largest basins: {basin_size_product}')

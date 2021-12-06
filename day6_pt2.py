total_fish = 0

input22 = open('input6.txt', 'r')
for line in input22:
    start_fish = [int(x) for x in line.split(',')]

from collections import Counter
count = Counter(start_fish)

count[0] = 0
count[6] = 0
count[7] = 0
count[8] = 0

for i in range(256):
    state = []
    for i in range(9):
        state.append(count.get(i,0))
        
    count[0] = state[1]
    count[1] = state[2]
    count[2] = state[3]
    count[3] = state[4]
    count[4] = state[5]
    count[5] = state[6]
    count[6] = state[7]+state[0]
    count[7] = state[8]
    count[8] = state[0]

for key, value in sorted(count.items(), key=lambda x: x[0]):
    total_fish += value

print(total_fish)

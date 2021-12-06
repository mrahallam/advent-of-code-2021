total_fish = 0

input22 = open('input6.txt', 'r')
for line in input22:
    start_fish = [int(x) for x in line.split(',')]

from collections import Counter
count = Counter(start_fish)

for i in range(256):
    state = []
    for i in range(9):
        state.append(count.get(i,0))
    for i in range(9):
        count[i] = count[i+1]
    count[6] = count[6]+state[0]
    count[8] = state[0]

for key, value in sorted(count.items(), key=lambda x: x[0]):
    total_fish += value

print(total_fish)

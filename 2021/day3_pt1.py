

with open('input3.txt') as file:
    lines = file.readlines()
    lines = [line.rstrip() for line in lines]

binary_length = len(lines[0])
length_input = len(lines)
counts = [0 for i in range(binary_length)]
gamma = counts[:]
epsilon = counts[:]
gamma_dec = 0
epsilon_dec = 0

for i,v in enumerate(counts):
    for j in lines:
        counts[i] += int(j[i])

for i,v in enumerate(gamma):
    if counts[i] > length_input / 2:
        gamma[i] = 1
        epsilon[i] = 0
    else:
        gamma[i] = 0
        epsilon[i] = 1

for bit in gamma:
    gamma_dec = (gamma_dec << 1) | bit

for bit in epsilon:
    epsilon_dec = (epsilon_dec << 1) | bit

print(gamma_dec*epsilon_dec)

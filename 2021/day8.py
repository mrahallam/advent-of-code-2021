#file_name = 'test8.txt'
file_name = 'input8.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [line.strip().split('|') for line in lines]
    lines = [[i.strip().split(' ') for i in line] for line in lines]

numbers = {1:0 ,4:0, 7:0, 8:0}

for i in lines:
    for j in i[1]:
        if(len(j)) == 2:
            numbers[1] += 1
        elif(len(j)) == 4:
            numbers[4] += 1
        elif(len(j)) == 3:
            numbers[7] += 1
        elif(len(j)) == 7:
            numbers[8] += 1

part_1 = 0

for k, v in numbers.items():
    print(k,'-->', v)
    part_1 += v

print(f'Part 1: {part_1}')

part_2 = 0
for i in lines:
    numbers_letters = {1:[], 4:[], 7:[], 8:[]}
    for j in i[0]:
        if(len(j)) == 2:
            numbers_letters[1] = set(j)
        elif(len(j)) == 4:
            numbers_letters[4] = set(j)
        elif(len(j)) == 3:
            numbers_letters[7] = set(j)
        elif(len(j)) == 7:
            numbers_letters[8] = set(j)
    
    print(f'for line: {i[1]}:')
    
    # 0,6,9 using 4, 1
    for j in i[0]:
        if(len(j)) == 6:
            if len(numbers_letters[4]-set(j)) == 0:
                numbers_letters[9] = set(j)
            else:
                if len(numbers_letters[1] - set(j)) == 0:
                    numbers_letters[0] = set(j)
                else:
                    numbers_letters[6] = set(j)

    # 2,3,5 using 4, 1
    for j in i[0]:
        if(len(j)) == 5:
            # of those length 5, 3 overlaps completely with 1
            if len(numbers_letters[1]-set(j)) == 0:
                numbers_letters[3] = set(j)
            else:
                if len(numbers_letters[4] - set(j)) == 1:
                    numbers_letters[5] = set(j)
                else:
                    numbers_letters[2] = set(j)

    for k, v in numbers_letters.items():
        print(k,'-->',v)

    output_number = ''

    for j in i[1]:
        for k, v in numbers_letters.items():
            
            if set(j) == v:
                output_number += str(k)

    part_2 += int(output_number)

print(part_2)
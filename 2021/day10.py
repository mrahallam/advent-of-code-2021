import statistics

#file_name = 'test10.txt'
file_name = 'input10.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [list((line.strip())) for line in lines]

legal_pairs = [('(',')',3,1),('[',']',57,2),('{','}',1197,3),('<','>',25137,4)]
points = dict([pair[1:2]+pair[2:3] for pair in legal_pairs])
score_multiplier = dict([pair[0:1]+pair[3:4] for pair in legal_pairs])

total_syntax_error = 0

def check_for_closed_pair(prev_char,char):
    if (prev_char,char) in [pair[0:2] for pair in legal_pairs]:
        return True

for line in lines[:]:
    check_line = []
    prev_char = ''

    for i, char in enumerate(line):
        check_line.append(char)
        if len(check_line) > 1:
            prev_char = check_line[-2]
        if check_for_closed_pair(prev_char,char):
            check_line = check_line[:-2]
        if len(check_line) > 1:
            if check_line[-1] in [pair[1] for pair in [pair[0:2] for pair in legal_pairs]]:
                total_syntax_error += points[check_line[-1]]
                lines.remove(line)
                break

print(f'The total syntax error score is: {total_syntax_error}')

total_scores = []

for line in lines:
    check_line = line
    while set(check_line).intersection(set([pair[1] for pair in [pair[0:2] for pair in legal_pairs]])):
        for i in range(len(check_line)-1,0,-1):
            if check_for_closed_pair(check_line[i-1],check_line[i]):
                check_line = check_line[0: i-1] + check_line[i + 1::]
                break
            else:
                continue
    check_line.reverse()

    total_score = 0
    for i in check_line:
        total_score *= 5
        total_score += score_multiplier[i]
    
    total_scores.append(total_score)

print(f'The middle score is: {statistics.median(total_scores)}')

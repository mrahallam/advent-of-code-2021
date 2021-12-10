import statistics

#file_name = 'test10.txt'
file_name = 'input10.txt'

with open(file_name) as file:
    lines = file.readlines()
    lines = [list((line.strip())) for line in lines]

legal_pairs = [('(',')'),('[',']'),('{','}'),('<','>')]
opening_characters = ['(','[','{','<']
closing_characters = [')',']','}','>']
points = {')':3,']':57,'}':1197,'>':25137}
score_multiplier = {'(':1,'[':2,'{':3,'<':4}

illegals = []
total_syntax_error = 0

def check_for_closed_pair(prev_char,char):
    if (prev_char,char) in legal_pairs:
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
            if check_line[-1] in closing_characters:
                #print(f'Line is {"".join(line)}, {check_line}, corrput')
                illegals.append(check_line[-1])
                lines.remove(line)
                break

for i in illegals:
    total_syntax_error += points[i]

print(f'The total syntax error score is: {total_syntax_error}')

total_scores = []

for line in lines[:]:
    check_line = line
    while set(check_line).intersection(set(closing_characters)):
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
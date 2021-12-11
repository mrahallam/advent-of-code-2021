import re

player_1 = []
player_2 = []
counter = 1

input22 = open('input22_2020.txt', 'r')
for line in input22:
    if re.match(r'^Player 1',line):
        counter = 1
    elif re.match(r'^Player 2',line):
        counter = 2
    else:
        if counter == 1:
            try:
                player_1.append(int(line))
            except Exception:
                pass
        else:
            try:
                player_2.append(int(line))
            except Exception:
                pass

def empty_list(list):
    if not list:
        return 1
    else:
        return 0


while not empty_list(player_1) and not empty_list(player_2):

    p1_card = player_1.pop(0)
    p2_card = player_2.pop(0)

    if p1_card > p2_card:
        cards = [p1_card,p2_card]
        player_1 += cards

    elif p2_card > p1_card:
        cards = [p2_card,p1_card]
        player_2 += cards

p1_score = 0
p2_score = 0
multiplier = 1

for i in reversed(player_1):
    p1_score += multiplier * i
    multiplier += 1

for i in reversed(player_2):
    p2_score += multiplier * i
    multiplier += 1

print(p1_score, p2_score)

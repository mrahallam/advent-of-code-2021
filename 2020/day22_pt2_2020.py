import re
import copy

player_1 = []
player_2 = []
counter = 1

input22 = open('input22_2020.txt', 'r')
#input22 = open('test22_2020.txt', 'r')
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

def play_game(deck1,deck2,get_winner = False):
    winner = None
    decks_this_game = set()
    while winner is None:
        deck_to_check = ",".join(str(x) for x in deck1)+",".join(str(x) for x in deck2)
        if deck_to_check in decks_this_game:
            winner = 1
            print('recursive escape!')
            break
        decks_this_game.add(deck_to_check)
        #print(f'{"".join(str(x) for x in deck1)}, {"".join(str(x) for x in deck2)}, {get_winner}')
        
        p1_card = deck1.pop(0)
        p2_card = deck2.pop(0)

        if p1_card <= len(deck1) and p2_card <= len(deck2):
            winner_of_round = play_game(deck1[0:p1_card],deck2[0:p2_card],True)
        elif p1_card > p2_card:
            winner_of_round = 1
        else:
            winner_of_round = 2

        if winner_of_round == 1:
            deck1 += [p1_card,p2_card]
            #print(f'deck 1: {"".join(str(x) for x in deck1)}')
            if len(deck2)==0:
                winner = 1
        else:
            deck2 += [p2_card,p1_card]
            #print(f'deck 2: {"".join(str(x) for x in deck2)}')
            if len(deck1)==0:
                winner = 2

    if get_winner:
        return winner
    else:
        if winner == 1:
            return get_score(deck1)
        elif winner == 2:
            return get_score(deck2)

def get_score(deck):
    score = 0
    multiplier = 1
    for i in reversed(deck):
        score += multiplier * i
        multiplier += 1
    return score

print(play_game(copy.deepcopy(player_1),copy.deepcopy(player_2)))
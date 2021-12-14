#file_name = 'test/test14.txt'
file_name = 'input/input14.txt'
from collections import Counter
import itertools as it

steps = 40

with open(file_name, 'r') as file:
    polymer_template, list_of_rules = data = file.read().strip().split("\n\n")
    insertion_rules = dict(rule.split(" -> ") for rule in list_of_rules.split("\n"))

pairs = {}

counts = {}

def insert_polymers(template,i):

    if i == 0:
        for j in range(1, len(template)):
            a,b = template[j-1],template[j]
            if not a+b in pairs:
                pairs[a+b] = 1
            else:
                pairs[a+b] += 1
    else:
        for k,v in list(pairs.items()):
            
            if k in insertion_rules:
                first_new = k[0] + insertion_rules[k]
                second_new =  insertion_rules[k] + k[1]
                if not first_new in pairs:
                    pairs[first_new] = 1 * v
                else:
                    pairs[first_new] += 1 * v
                if not second_new in pairs:
                    pairs[second_new] = 1 * v
                else:
                    pairs[second_new] += 1 * v
                pairs[k] -= 1 * v

for i in range(steps+1):
    insert_polymers(polymer_template,i)

for k,v in pairs.items():
    if not k[0] in counts:
        counts[k[0]] = 1 * v
    else:
        counts[k[0]] += 1 * v

if not polymer_template[-1] in counts:
    counts[polymer_template[-1]] = 1
else:
    counts[polymer_template[-1]] += 1

key_min = min(counts.keys(), key=(lambda k: counts[k]))
key_max = max(counts.keys(), key=(lambda k: counts[k]))

print(f'difference between {counts[key_max]} and {counts[key_min]} is {counts[key_max] - counts[key_min]}')

import pandas as pd
import numpy as np
df = pd.read_csv('C:/Users/AJH/source/repos/TestPythonApplication/TestPythonApplication/input.txt',header=None,names=["Sequence"])
df['match'] = df.Sequence > df.Sequence.shift()
df['next_3'] = df['Sequence'].rolling(3).sum().shift(-3)
df['match2'] = df.next_3 > df.next_3.shift()

part_1 = df[['Sequence','match']]
part_2 = df[['Sequence', 'match2']]
table = pd.pivot_table(part_1[part_1.match == True], index=['match'], aggfunc='count') 
table2 = pd.pivot_table(part_2[part_2.match2 == True] ,index=['match2'], aggfunc='count')

part1 = df.match.sum()
part2 = df.match2.sum()

print(table)
print(table2)

print(f'Part One = {part1}, Part Two = {part2}')

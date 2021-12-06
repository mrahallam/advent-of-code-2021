import re
import pandas as pd
import numpy as np
df = pd.read_csv('C:/Users/AJH/source/repos/TestPythonApplication/TestPythonApplication/input2.txt',header=None,names=["directions"])

horizontal = 0
depth = 0
aim = 0

for row in df.itertuples(index=True, name='directions'):
    i = int(re.match('.*?([0-9]+)$', row.directions).group(1))
    s = row.directions.split()[0]
    if s == 'up':
        aim -= i
    elif s == 'down':
        aim += i
    elif s == 'forward':
        horizontal += i
        depth += aim * i

print(horizontal * depth)

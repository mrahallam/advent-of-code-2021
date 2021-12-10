with open("input4.txt") as f:
  called_numbers = f.readline().strip().split(',')
  boards = [[[[x, False] for x in line.split()] for line in board.split('\n')] for board in f.read().strip().split('\n\n')]

def check_for_win(board,row,column):
  win = False
  column_sum = 0
  row_sum = 0
  for numbers in row:
    row_sum += numbers[1]
  for rows in board:
    column_sum += rows[column][1]
  win = column_sum == 5 or row_sum == 5
  return (win,row_sum,column_sum)

def get_score(board,called_number):
  score = 0
  for row in board:
    for column in row:
      if not column[1]:
        score += int(column[0])
  score *= int(called_number)
  return score

for called_number in called_numbers:
  for board_index, board in enumerate(boards):
    for row in board:
      for col_index, column in enumerate(row):
        if column[0] == called_number:
          column[1] = True
          if check_for_win(board,row,col_index)[0]:
            print(f'Winner is board {board_index} with a score of {get_score(board,called_number)}!')
            for r in board:
              print(r)
            break
      else:
        continue
      break
    else:
      continue
    break
  else:
    continue
  break

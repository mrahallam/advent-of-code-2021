file_name = 'test18.txt'
file_name = 'input18.txt'

import math
import re

with open(file_name) as file:
    lines = file.readlines()
    lines = [line.strip() for line in lines]

def get_mag(final_snailfish_number):
    open_bracket_count = final_snailfish_number.count('[')
    while open_bracket_count > 0:
        for index,value in enumerate(final_snailfish_number):
            if value == ']':
                left_index = final_snailfish_number[:index].rfind('[')
                pair = final_snailfish_number[left_index+1:index]
                numbers = re.findall('[+-]?\d+(?:\.\d+)?', pair)
                pair_result = (3*int(numbers[0])+2*int(numbers[1]))
                final_snailfish_number = final_snailfish_number[:left_index] + str(pair_result) + final_snailfish_number[index+1:]
                open_bracket_count = final_snailfish_number.count('[')
                break
    return final_snailfish_number    

def add_snailfish(s1,s2):
    output = f'[{s1},{s2}]'
    return output

def reduce(snailfish_number):
        
    done_something = True
    while done_something == True:
        done_something = False
        counter = 0
        for j,v1 in enumerate(snailfish_number):
            if v1 == '[':
                counter += 1
            if v1 == ']':
                counter -= 1
            if counter > 4:
                left_str = snailfish_number[:j]
                left_numbers = re.findall('[+-]?\d+(?:\.\d+)?', snailfish_number[j+1:])
                left_pair = left_numbers[0]
                left_numbers = re.findall('[+-]?\d+(?:\.\d+)?',left_str)
                if left_numbers:
                    rightmost_left = left_numbers[-1]
                    rightmost_left_index = left_str.rfind(rightmost_left)
                    rightmost_left_len = len(left_numbers[-1])
                    the_left_side =left_str[:rightmost_left_index] + str(int(left_pair)+int(rightmost_left))+left_str[rightmost_left_index+rightmost_left_len:]
                else:
                    the_left_side = snailfish_number[:j]
                right_str = snailfish_number[j:]
                comma_index = right_str.find(',')
                right_str = right_str[comma_index+1:]
                numbers_right = re.findall('[+-]?\d+(?:\.\d+)?', right_str)
                right_pair = numbers_right[0]
                new_right_str = right_str[len(right_pair):]
                numbers_right = re.findall('[+-]?\d+(?:\.\d+)?', new_right_str)
                if numbers_right:
                    index_next_right = new_right_str.find(numbers_right[0])
                    len_next_right = len(numbers_right[0])
                    middle_bit = new_right_str[1:index_next_right]
                    new_new_right_str = new_right_str[index_next_right+len_next_right:]
                    the_right_side = middle_bit+str(int(right_pair)+int(numbers_right[0]))+new_new_right_str
                else:
                    the_right_side = new_right_str[1:]
                snailfish_number=the_left_side+'0'+the_right_side                
                counter = 0
                done_something=True
                break

            if j == len(snailfish_number)-1:
                numbers = re.findall('[+-]?\d+(?:\.\d+)?', snailfish_number)
                numbers_to_split = []
                for number in numbers:
                    if int(number) > 9:
                        numbers_to_split.append(number)
                if numbers_to_split:
                    num_to_split = numbers_to_split[0]
                    len_of_num = len(numbers_to_split[0])
                    index_of_num = snailfish_number.find(numbers_to_split[0])

                    left_element = math.floor(int(num_to_split)/2)
                    right_element = math.ceil(int(num_to_split)/2)
                    new_pair = '['+ str(left_element) + ',' + str(right_element) + ']'

                    snailfish_number =  snailfish_number[:index_of_num] + new_pair + snailfish_number[index_of_num+len_of_num:]
                    done_something = True
                else:
                    return snailfish_number
                break

#Part 1
intermediate_result = ''
for i, v in enumerate(lines):
    if i == 0:
        intermediate_result = reduce(add_snailfish(lines[i],lines[i+1]))
    elif i > 1:
        intermediate_result = reduce(add_snailfish(intermediate_result,lines[i]))

print(f'magnitude of the final sum: {get_mag(intermediate_result)}')

#Part 2
array_of_two_numbers = []
for i,i_value in enumerate(lines):
    for j, j_value in enumerate(lines):
        if j != i:
            array_of_two_numbers.append(reduce(add_snailfish(i_value,j_value)))
            array_of_two_numbers.append(reduce(add_snailfish(j_value,i_value)))

magnitudes = []

for number in array_of_two_numbers:
    magnitudes.append(int(get_mag(number)))

print(f'largest magnitude of any sum of two different snailfish numbers: {max(magnitudes)}')
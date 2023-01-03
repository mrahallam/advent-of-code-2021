import ast
import math
import numpy as np

with open("InputData13.txt") as reader:
	lines = reader.readlines()

input = []
for line in lines:
	line = line.strip()
	if line != "":
		input.append(line)


def xor(x,y):
	return bool((x and not y) or (not x and y))

def compare(item1,item2):
	if isinstance(item1,int):
		print("enter")
		if isinstance(item2,int):
			return compare_ints(item1,item2)
		if isinstance(item2,list):
			return single_int(item1,item2)
	if isinstance(item1,list):
		#print("enter2")
		if isinstance(item2,int):
			return single_int(item1,item2)
		if isinstance(item2,list):
			return compare_lists(item1,item2)

def compare_ints(int1,int2):
	if int1 < int2:
		return True
	elif int1 > int2:
		return False
	else:
		return None

def compare_lists(list1,list2):
	#print("enter compare lists")
	#print(list1)
	#print(list2)
	if xor(len(list1) == 0, len(list2) == 0):
		if len(list1) == 0:
			return True
		else:
			return False
	if len(list1) == 0 and len(list2) == 0:
		#go back out and somehow check the next entry
		return None
	item1 = list1[0]
	item2 = list2[0]
	
	#print(item1)
	#print(item2)
	if isinstance(item1,int):
		if isinstance(item2,int):
			test = compare_ints(item1,item2)
			if test:
				return True
			elif test == False:
				return False
			else:
				value =  compare_lists(list1[1:len(list1)],list2[1:len(list2)])
				if value:
					return True
				elif value == False:
					return False
		if isinstance(item2,list):
			#return compare_lists([item1],item2)
			if [item1] == item2:
				return compare(list1[1:len(list1)],list2[1:len(list2)])
			else:
				return compare_lists([item1],item2)
			
	if isinstance(item1,list):
		if isinstance(item2,int):
			#return compare_lists(item1,[item2])
			if item1 == [item2]:
				return compare(list1[1:len(list1)],list2[1:len(list2)])
			else:
				return compare_lists(item1,[item2])			
		if isinstance(item2,list):
			if item1 == item2:
				return compare_lists(list1[1:len(list1)],list2[1:len(list2)])
			else:
				return compare_lists(item1,item2)


def single_int(left,right):
	print("enter single")
	if isinstance(left,int):
		return compare_lists([left],right)
	else:
		return compare_lists(left,[right])

sum = 0
for x in range(0,len(input), 3):
	line1 = ast.literal_eval(input[x])
	line2 = ast.literal_eval(input[x+1])
	index = math.floor((x)/3)+1
	correct_order = compare(line1,line2)
	print(correct_order)
	if correct_order:
		print("index is " + str(index))
		sum = sum + index
		
print(sum)

ordered_list = [[[2]],[[6]]]
for x in range(len(input)):
	line = ast.literal_eval(input[x])
	for y in range(len(ordered_list)):
		packet = ordered_list[y]
		order = compare(packet, line)
		if order == False:
			ordered_list.insert(y,line)
			break
		if y == len(ordered_list)-1:
			ordered_list.append(line)
'''
print("       ")
for packet in ordered_list:
	print(packet)
'''
index_1 = ordered_list.index([[2]])+1
index_2 = ordered_list.index([[6]])+1
product = index_1 * index_2
print(product)


	
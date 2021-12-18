file_name = 'test16.txt'
file_name = 'input16.txt'
import numpy as np
import math

with open(file_name) as file:
    line = file.readline()

transmission = "{0:08b}".format(int(line, 16))
for i in range(len(line)*4-len(transmission)):
    transmission = '0' + transmission

print(transmission)

version_sum = 0

def packet_resolution(input_list, type_id):
    if type_id == 0:
       return sum(input_list)
    elif type_id == 1:
        return math.prod(input_list)
    elif type_id== 2:
        return min(input_list)
    elif type_id ==3:
        return max(input_list)
    elif type_id == 5:
        return 1 if input_list[0]>input_list[1] else 0
    elif type_id == 6:
        return 1 if input_list[0]<input_list[1] else 0
    elif type_id == 7:
        return 1 if input_list[0]==input_list[1] else 0

def parse_transmission(input):
    global version_sum
    version = int(int(input[:3],2))
    version_sum += version
    input = input[3:]
    packet_type_id = int(input[:3],2)
    input = input[3:]
    print(version,packet_type_id,input)
    #literal containing packet
    if packet_type_id == 4:
        literal = ''
        last_packet = False
        while not last_packet:
            packet = input[:5]
            input = input[5:]
            if packet[0] == '0':
                last_packet = True
                literal += packet[1:]
                print(f'packet contains literal value: {int(literal,2)}')
            else:
                print(f'{packet[1:]} is not the last packet')
                literal += packet[1:]
        return input,int(literal,2)
    else:
        length_type_id = input[0]
        input = input[1:]
        sub_packet_literals = []
        if length_type_id == '0':
            length_sub_packets = int(input[:15],2)
            input = input[15:]
            sub_packets_of_length = input[:length_sub_packets]
            print(f'length of sub packet is {length_sub_packets}, sub packet is {sub_packets_of_length}')
            while sub_packets_of_length:
                sub_packets_of_length,sub_literal = parse_transmission(sub_packets_of_length)
                sub_packet_literals.append(sub_literal)
            input = input[length_sub_packets:]
        else:
            count_sub_packets = int(input[:11],2)
            input = input[11:]
            print(f'count of sub packets is {count_sub_packets}')
            for i in range(count_sub_packets):
                sub_packets_of_count, sub_literal = parse_transmission(input)
                input = sub_packets_of_count
                sub_packet_literals.append(sub_literal)
        return input, packet_resolution(sub_packet_literals,packet_type_id)
            
print(f'final result is: {parse_transmission(transmission)[1]}')
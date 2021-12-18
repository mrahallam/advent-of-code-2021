file_name = 'test16.txt'
file_name = 'input16.txt'

with open(file_name) as file:
    line = file.readline()

transmission = "{0:08b}".format(int(line, 16))
zero_to_add = len(line)*4-len(transmission)

for i in range(zero_to_add):
    transmission = '0' + transmission

packet_version = ''
packet_type_id=''
length_type_id=''
version_total = 0
i=0

def binary_to_decimal(binary_input):
    decimal_output = int(binary_input, 2)
    return decimal_output

def get_version(input):
    packet_version = binary_to_decimal(input)
    return packet_version

def get_packet_type_id(input):
    packet_type_id = binary_to_decimal(input)
    return packet_type_id

def get_packet(input):
    packet = input[:5]
    return packet

def at_the_end(input):
    bit = True
    for i in input:
        if i == '1':
            bit = False
            break
    return bit

literal = ''
packet_type_id = ''
packet_operator = ''
packet_result = 0
previous_result = 0

def parse_packets(input,decrement_counter=False,counter=0,decrement_sub_counter=False,sub_counter=0,type_id=None):

    global i
    global version_total
    global literal
    global packet_type_id
    global packet_result

    if decrement_counter == True and counter == 0:
        print('Reached end of sub packets, decrementing total length of packets')
        previous_result = packet_result
        packet_result = 0
        decrement_counter = False
        if at_the_end(input[i:]):
            return 0
    if decrement_sub_counter == True and sub_counter == 0:
        print('Reached end of sub packets, decrementing number of packets')
        previous_result = packet_result
        packet_result = 0
        decrement_sub_counter = False
        if at_the_end(input[i:]):
            return 0
    version = get_version(input[i:i+3])
    version_total += version
    i+=3
    if decrement_counter == True:
        counter -= 3
    packet_type_id = get_packet_type_id(input[i:i+3])
    print(f'i is {i}, packet type id is {packet_type_id}, binary is {input[i:i+3]}')
    i+=3
    if decrement_counter == True:
        counter -= 3
    last_packet = False
    length_contained_sub_packets = 0
    number_contained_sub_packets = 0
    print(version,packet_type_id)
    if packet_type_id == 4:
        literal = ''
        while not last_packet:
            packet = get_packet(input[i:])
            print(packet)
            if packet[0] == '0':
                last_packet = True
                print(f'last packet = {packet[1:]}')
                literal += packet[1:]
                print(f'packet contains literal value: {binary_to_decimal(literal)}')
                i += 5
                if decrement_counter == True and sub_counter == False:
                    counter -= 5
                    parse_packets(input,decrement_counter=True,counter=counter,type_id= type_id)
                elif decrement_sub_counter == True and decrement_counter == False:
                    sub_counter -= 1
                    parse_packets(input,decrement_sub_counter=True,sub_counter=sub_counter,type_id= type_id)
                else:
                    parse_packets(input)
            else:
                print(f'{packet[1:]} is not the last packet')
                literal += packet[1:]
                i+=5
                if decrement_counter == True:
                    counter -= 5
        
    if packet_type_id != 4:
        length_type_id = input[i]
        print(f'operator packet, operator type = {packet_type_id}')
        print(f'length type id is: {input[i]}')
        if length_type_id == '0': #length is a 15-bit number, total length in bits of contained sub packets
            i+=1
            length_contained_sub_packets = binary_to_decimal(input[i:i+15])
            print(f'sub packets of total length {length_contained_sub_packets} to follow..')
            i+=15
            print(f'we need to stop when we get to i = {i+length_contained_sub_packets}')
            parse_packets(input,decrement_counter = True,counter=length_contained_sub_packets,type_id = packet_type_id)
        elif length_type_id == '1': #length is a 11-bit number, number of contained sub packets
            i+=1
            number_contained_sub_packets = binary_to_decimal(input[i:i+11])
            print(f'{number_contained_sub_packets} sub packets to follow..')
            i+=11
            print(f'we need to stop when we have processed {number_contained_sub_packets} sub packets')
            parse_packets(input,decrement_sub_counter=True,sub_counter=number_contained_sub_packets,type_id = packet_type_id)


literal = parse_packets(transmission[i:])
print(f'version total is {version_total}')
with open('input3.txt') as file:
    lines = file.readlines()
    lines = [line.rstrip() for line in lines]
oxygen = lines[:]
co2 = lines[:]

oxygen_generator_rating = 0
co2_scrubber_rating = 0

def count_at_position(lines,position):
    t,f = 0, 0
    for i in lines:
        if i[position] == '1':
            t += 1
        else:
            f += 1
    return (t,f)

def binary_string_to_dec(binary):
    dec = int(binary, 2)
    return dec

while len(oxygen) > 1:
    for i, v in enumerate(oxygen[0]):
        t,f = count_at_position(oxygen,i)
        bit = t >= f
        if len(oxygen) > 1:
            oxygen = [ elem for elem in oxygen if elem[i] == str(int(bit))]
        oxygen_generator_rating = binary_string_to_dec(oxygen[0])

for i, v in enumerate(co2[0]):
    t,f = count_at_position(co2,i)
    bit = t < f
    if len(co2) > 1:
        co2 = [ elem for elem in co2 if elem[i] == str(int(bit))]
    co2_scrubber_rating = binary_string_to_dec(co2[0])

print(co2_scrubber_rating*oxygen_generator_rating)

# Day Thirteen

## Prep

def compare(l1, l2):
    for e1, e2 in zip(l1, l2):
        if isinstance(e1, int) and isinstance(e2, int):
            if e1 < e2:
                return 1
            if e1 > e2:
                return -1
        elif isinstance(e1, list) and isinstance(e2, list):
            result = compare(e1, e2)
            if result != 0:
                return result
        elif isinstance(e1, int): # e1 is int; e2 is list
            result = compare([e1], e2)
            if result != 0:
                return result
        else: # e1 is int; e2 is list
            result = compare(e1, [e2])
            if result != 0:
                return result
    
    # if we reached this point, no differences were found.
    if len(l1) < len(l2): # left side ran out of items first
        return 1
    elif len(l1) > len(l2): # right side ran out of items first
        return -1
    
    return 0

packets = {
    i: (eval(p[0]), eval(p[1]))
    for i, p in enumerate([p.splitlines() for p in open("InputData13.txt").read().split("\n\n")], 1)
}


## Part One

print(sum(key for key, value in packets.items() if compare(value[0], value[1]) == 1))


## Part Two

all_packets = [u for v in list(packets.values()) for u in v] + [[[2]], [[6]]]
print((1 + sum(1 for p in all_packets if compare(p, [[2]]) == 1)) * (1 + sum(1 for p in all_packets if compare(p, [[6]]) == 1)))
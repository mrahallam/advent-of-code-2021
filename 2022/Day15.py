import re
locs=[list(map(int,re.findall(r'(-?[0-9]+)',y)))for y in open('InputData15.txt').read().split('\n')]
dists=[abs(l[0]-l[2])+abs(l[1]-l[3]) for l in locs]
b,n=4000000+1,0
found=False
for y in range(b):
    segs=[]
    for i in range(len(locs)):
        l,d=locs[i],dists[i]
        h=abs(y-l[1])
        if h<=d:
            segs.append([max(0,l[0]-(d-h)),min(l[0]+(d-h),b)])
    segs=sorted(segs)
    curmax=segs[0][1]
    for i in range(len(segs)-1):
        curmax=max(curmax,segs[i][1])
        if curmax+1<segs[i+1][0]:
            print(segs[i][1]+1)
            print(y)
            print(4000000*(segs[i][1]+1)+y)
            found=True
    if found:
        break
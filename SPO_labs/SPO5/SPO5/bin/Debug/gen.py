import random

f = open("test.txt","w")

outp = " ".join(["".join([chr(random.randint(34, 126)) for j in range(32)]) for i in range(1000)])
f.write(outp)
f.close()
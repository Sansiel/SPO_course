from os import listdir, path, stat
from os.path import isfile, join
from stat import *
import os
import argparse
import datetime
from math import ceil

parser = argparse.ArgumentParser()
parser.add_argument("-a", "--all", help="list all files/directories", action="store_true")
parser.add_argument("-l", "--large", help="list files/directories verbose", action="store_true")
parser.add_argument("-C", help="list entries by columns", action="store_true")
parser.add_argument("path", type=str, help="path to directory")
args = parser.parse_args()
print(args.path)

files = listdir(args.path)
column=[]
N=5
if args.large: print ("total {}".format(len(files)))

if args.all: 
    files.append("..") 
    files.append(".")
   
for f in files:
    if args.large:
        stmod = stat(f).st_mode
        fmod=[]
        if S_ISDIR(stmod): fmod.append("d") 
        else: fmod.append("-")

        if bool(stmod & S_IRUSR): fmod.append("r") 
        else: fmod.append("-")
        if bool(stmod & S_IWUSR): fmod.append("w") 
        else: fmod.append("-")
        if bool(stmod & S_IXUSR): fmod.append("x") 
        else: fmod.append("-")
        if bool(stmod & S_IRGRP) : fmod.append("r")
        else: fmod.append("-")
        if bool(stmod & S_IWGRP): fmod.append("w") 
        else: fmod.append("-")
        if bool(stmod & S_IXGRP): fmod.append("x") 
        else: fmod.append("-")
        if bool(stmod & S_IROTH): fmod.append("r") 
        else: fmod.append("-")
        if bool(stmod & S_IWOTH): fmod.append("w") 
        else: fmod.append("-")
        if bool(stmod & S_IXOTH): fmod.append("x") 
        else: fmod.append("-")

        creator = stat(f).st_uid #pwd don't want work
        size = stat(f).st_size
        date = datetime.datetime.fromtimestamp(path.getmtime(f))
        print("{}\t{}\t{}\t{}\t{}\t{}\t".format(fmod, stmod.denominator, creator, size, date, f))
    
    elif args.C: 
        column.append(f)
    else: print(f)

def parting(xs, parts):
    part_len = ceil(len(xs)/parts)
    return [xs[part_len*k:part_len*(k+1)] for k in range(parts)]
    
if args.C: print (*column)
    # for i in range(0,len(column),len(column)//10):
    #     print('{0:4}{1:4}{2:4}{3:4}{4:4}{5:4}{6:4}{7:4}{8:4}{9:4}'.format(*column[i:i+10]))
    # columns = parting(column,N)
    # ans_comlumns=[]
    # for i in range (0,N):
    #     ans_comlumns.append([])
    #     for c in columns[i]:
    #         ans_comlumns[i].append(c)
    # for ac in ans_comlumns: print (*ac)


 #-*- coding: utf-8 -*-
import json
import sys
print(sys.argv)

path = sys.argv[1] + "/" + sys.argv[2]
wPath = sys.argv[1] + "/key.txt"
wPath2 = sys.argv[1] + "/value.txt"

jsonData = {}
keys = ""
vals = ""

def Parsing():
    with open(path,'rt', encoding='UTF-8') as json_data:
        jsonData = json.load(json_data)

    global keys
    global vals

    for key, val in dict.items(jsonData):
        keys += (key + " ")
        vals += (str(val)) + ":" + str(type(val)) + "\n"
    
    json_data.close()

def WriteKeyFile():
    with open(wPath,'wt', encoding='UTF-8') as write_data:
        write_data.write(keys)
    
    write_data.close()

def WriteValueFile():
    with open(wPath2,'wt', encoding='UTF-8') as write_data:
        write_data.write(vals)

    write_data.close()      

Parsing()
WriteKeyFile()
WriteValueFile()
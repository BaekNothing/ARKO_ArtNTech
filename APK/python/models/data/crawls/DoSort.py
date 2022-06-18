import os
import csv
import openpyxl

#read textfile and write to csv
def remove_empty_line(string):
    return string.replace('\n\n', '\n')
def remove_comma(string):
    return string.replace(',', '')

def read_textfile_write_csv(filepath, filename):
    filename = filepath + '/' + filename
    with open(filename, 'r', encoding='utf8') as f:
        text = f.read()
    text = remove_empty_line(text)
    text = remove_comma(text)
    splice = text.split("********")
    with open('result.csv', 'a', encoding='utf-8-sig', newline='') as f:
        writer = csv.writer(f)
        writer.writerow(splice)

def read_All_textfile_write_csv(filepath):
    listdir = os.listdir(filepath)
    for i in listdir:
        if i.endswith('.txt'):
            print(i)
            read_textfile_write_csv(filepath, i)

name = input('filename: ')
if name == 'all':
    read_All_textfile_write_csv('results/archives')
else:
    try:
        read_textfile_write_csv('results/archives', name)
    except Exception as e:
        print(e)
    
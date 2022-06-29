import os
import csv
import openpyxl

#read textfile and write to csv
def remove_empty_line(string):
    return string.replace('\n\n', '\n')
def remove_comma(string):
    return string.replace(',', '')

def read_textfile_write_csv(filepath, filename):
    fileId = filename[0:4]
    filename = filepath + '/' + filename
    with open(filename, 'r', encoding='utf8') as f:
        text = f.read()
    text = remove_empty_line(text)
    text = remove_comma(text)
    splice = text.split("********")
    
    with open('result' + fileId + '.csv', 'a', encoding='utf-8-sig', newline='') as f:
        writer = csv.writer(f)
        writer.writerow(splice)

def read_All_textfile_write_csv(filepath):
    listdir = os.listdir(filepath)
    for i in listdir:
        if i.endswith('.txt'):
            print(i)
            read_textfile_write_csv(filepath, i)


def add_column_to_csv():
    with open('result.csv', 'r', encoding='utf-8-sig') as f:
        text = f.read()
    csv_reader = csv.reader(text.splitlines())
    with open('result_addCategory.csv', 'w', encoding='utf-8-sig', newline='') as f:
        writer = csv.writer(f)
        for row in csv_reader:
            row.append(col_classify(row[2]))
            writer.writerow(row)


def col_classify(str):
    if "展" in str :
        return "exhibition"
    
    elif "모집" in str :
        return "recruitment"
    elif "공모" in str :
        return "recruitment"
    elif "공고" in str :
        return "recruitment"
    elif "어워드" in str :
        return "recruitment"
    elif "대상" in str :
        return "recruitment"
    
    else :
        return "other"

name = input('filename: ')
if name == 'all':
    read_All_textfile_write_csv('results/archives')
elif name == 'add_column':
    add_column_to_csv()
else:
    try:
        read_textfile_write_csv('results/archives', name)
    except Exception as e:
        print(e)
    
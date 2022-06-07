from operator import index
from turtle import delay
import urllib.request  
from bs4 import BeautifulSoup  
import os
import time

def get_index():
    url = 'https://neolook.com/archives'
    conn = urllib.request.urlopen(url)
    soup = BeautifulSoup(conn, 'html.parser')
    arta = soup.find_all('li', class_='px-1')
    index = ""
    for art in arta:
        index += art.find('a')["href"] + '\n'
    print(index)
    with open('cwals/index/index.txt', 'wt', encoding='utf8') as f:
        f.write(str(index))
    return index

def get_subIndex(url):
    conn = urllib.request.urlopen('https://neolook.com' + url)
    soup = BeautifulSoup(conn, 'html.parser')
    arta = soup.find_all('li', class_='py-1')
    subIndex = ""
    for art in arta:
        subIndex += art.find('a')["href"] + '\n'
    print(subIndex)
    with open('cwals/index/' + url + '.txt', 'wt', encoding='utf8') as f:
        f.write(str(subIndex))
    return subIndex

def get_text_from_index():
    listdir = os.listdir('cwals/index/archives')
    for i in listdir:
        if i.endswith('.txt'):
            urls = open('cwals/index/archives/' + i, 'r', encoding='utf8').read().splitlines()
            for url in urls :
                try:
                    get_text(url)
                except:
                    print("error :", url)
                time.sleep(1)

def get_text(url):
    fname = 'cwals/results' + url + '.txt'
    if os.path.isfile(fname) :
        return 
    print(fname)
    conn = urllib.request.urlopen('https://neolook.com' + url)
    soup = BeautifulSoup(conn, "html.parser")  
    arta = soup.find_all('div', class_='archives-description')

    filteredText = url + '\n'
    for art in arta:
        filteredText += art.text.strip()
    filteredText = filteredText.replace(',', '')
    filteredText = filteredText.replace('\n\n\n', ',    ')

    with open(fname, 'wt', encoding='utf8') as f:
        f.write(str(filteredText))

text = input('keyword: ')
if  text.lower() == 'index':
    get_index()
elif  text.lower() == 'url':
    get_text(input('url: ')) 
elif  text.lower() == 'subindex':
    with open('cwals/index.txt', 'rt', encoding='utf8') as f:
        lines = f.read().splitlines()
        for line in lines:
            get_subIndex(line)
elif  text.lower() == 'text':
    get_text_from_index()
else:
    exit(print('keyward error'))
#출처: https://crazyj.tistory.com/190 [크레이지J의 탐구생활:티스토리]
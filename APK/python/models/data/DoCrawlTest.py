from cmath import e
from operator import index
import string
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
    with open('crawls/index/index.txt', 'wt', encoding='utf8') as f:
        f.write(str(index))
    return index

def get_subIndex(url):
    for i in range(1, 13): # 1 ~ 12
        try:
            new_url = url + ('0' if i < 10 else '') + str(i)
            conn = urllib.request.urlopen('https://neolook.com' + new_url)
            soup = BeautifulSoup(conn, 'html.parser')
            arta = soup.find_all('li', class_='py-1')
            subIndex = ""
            for art in arta:
                subIndex += art.find('a')["href"] + '\n'
            print(subIndex)
            with open('crawls/index/' + new_url + '.txt', 'wt', encoding='utf8') as f:
                f.write(str(subIndex))
            subIndex
        except Exception as e:
            print("error :", url, e)
            continue

def get_text_from_index():
    listdir = os.listdir('crawls/index/archives')
    for i in listdir:
        if i.endswith('.txt'):
            urls = open('crawls/index/archives/' + i, 'r', encoding='utf8').read().splitlines()
            for url in urls :
                try:
                    get_text(url)
                except Exception as e:
                    print("error :", url, e)
                time.sleep(0.05)

def get_text(url):
    fname = 'crawls/results' + url + '.txt'
    if os.path.isfile(fname) :
        return 
    print(fname)
    conn = urllib.request.urlopen('https://neolook.com' + url)
    soup = BeautifulSoup(conn, "html.parser")  
    arta = soup.find_all('div', class_='archives-description')

    filteredText = url + '\n'
    filteredText += "\n******** description ********\n" 
    for art in arta:
        filteredText += art.text.strip()
    filteredText = filteredText.replace(',', '')
    filteredText = filteredText.replace('\n\n\n', ',    ')
    
    filteredText += "\n******** imgSrcs ********\n"
    imgs = soup.find_all('img')
    for img in imgs:
        if "archives" in img["src"]:
            filteredText += img["src"] + '\n'
    
    print(filteredText)
    with open(fname, 'wt', encoding='utf8') as f:
        f.write(str(filteredText))

text = input('keyword: ')
if  text.lower() == 'index':
    get_index()
elif  text.lower() == 'url':
    get_text(input('url: ')) 
elif  text.lower() == 'subindex':
    with open('crawls/index/index.txt', 'rt', encoding='utf8') as f:
        lines = f.read().splitlines()
        for line in lines:
            get_subIndex(line)
elif  text.lower() == 'text':
    get_text_from_index()
else:
    exit(print('keyward error'))
#출처: https://crazyj.tistory.com/190 [크레이지J의 탐구생활:티스토리]
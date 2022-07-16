from ast import pattern
import os
import csv
import re

#read textfile and write to csv
def remove_comma(string):
    return string.replace(',', '***')
def remove_non_breakingSpace(string) :
    return string.replace('\xa0', '')
def remove_doubleEnter(string):
    return string.replace('\n\n','\n')

def read_textfile_write_csv(filepath, filename):
    fileId = filename[0:4]
    filename = filepath + '/' + filename
    with open(filename, 'r', encoding='utf8') as f:
        text = f.read()
    text = remove_comma(text)
    text = remove_non_breakingSpace(text)
    text = remove_doubleEnter(text)
    splice = text.split("******")
    #splice = filename, title, Title, Desc, desc, Imgs, imgs
    #print(splice[0]) #filename
    #print(splice[2].replace('\n\n','').replace('\n \n','')) #title
    #print(splice[4]) #desc
    #print(splice[6]) #imgs

    articleTitle = splice[2].split('\n')
    for title in articleTitle:
        if len(title) <= 0:
            articleTitle.remove(title)
    #articleTitle = 전시제목, 작가이름, 기간
    if len(articleTitle) <= 1 :
        return

    articleDesc = splice[4].split('\n')
    for desc in articleDesc:
        if len(desc) <= 0:
            articleDesc.remove(desc)
    # while all childs in articleDesc are length more than 25, merge them
    counter = 0
    while counter + 1 < len(articleDesc):
        if len(articleDesc[counter + 1]) < 25:
            articleDesc[counter] = articleDesc[counter] + '___' + articleDesc[counter + 1]
            articleDesc.pop(counter + 1)
        else:
            counter += 1

    #articleDesc = 전시 텍스트들
    #print(articleDesc)

    articleImgs = splice[6].split('\n')
    for img in articleImgs:
        if len(img) <= 0:
            articleImgs.remove(img)

    structedArticleImgs = []
    for img in articleImgs:
        #진원장_꿈_유화_60.5×72.5cm_1990***https://neolook.com/archives/1993020914a.jpg
        #imgStruct = name,title,media,size,date,url
        if len(img.split('***')) <= 1 :
            continue
        imgStruct = img.split('***')[0].split('_')
        imgStruct.append(img.split('***')[1])
        if(len(imgStruct) == 6):
            structedArticleImgs.append(imgStruct)
    #articleImgs = 전시 이미지들

    articleIndexName = splice[0].replace(',', '').replace('\n', '').replace('/archives/','')

    #descCol = id, articleId, title, genre, Text
    descHeadText = [articleIndexName, articleTitle[1],
                    articleTitle[0], col_classify(articleTitle[1])]

    #imgsCol = id, articleId, title, genre, imgTitle, media, size, year, src
    imgsHeadText = [articleIndexName, articleTitle[1],
                    articleTitle[0], col_classify(articleTitle[1])]

    with open('result' + fileId + '_desc.csv', 'a', encoding='utf-8-sig', newline='') as f:
        writer = csv.writer(f)
        for text in articleDesc:
            descText = []
            for head in descHeadText:
                descText.append(head)
            descText.append(text)
            writer.writerow(descText)

    with open('result' + fileId + '_imgs.csv', 'a', encoding='utf-8-sig', newline='') as f:
        writer = csv.writer(f)
        for structs in structedArticleImgs:
            imgsText = []
            for head in imgsHeadText:
                imgsText.append(head)
            for text in structs :
                imgsText.append(text)
            writer.writerow(imgsText)

def read_All_textfile_write_csv(filepath, year):
    listdir = os.listdir(filepath)
    for i in listdir:
        #year에 매칭되는 파일을 찾아서 읽어옴
        #if str(i).find(year) != -1 :
        if i.endswith('.txt'):
            # print(i)
            read_textfile_write_csv(filepath, i)


def col_classify(str):
    genreList = [
        "painting", 
        "drawing", 
        "sculpture", 
        "craft", 
        "ceramic", 
        "photography", 
        "installation", 
        "media art", 
        "mixed media", 
        "media", 
        "conceptual art", 
        "video art", 
        "video", 
        "digital painting", 
        "performance", 
        "sound",
        "개인전" ]
    
    for i in genreList:
        if str.find(i) != -1 :
            return "solo exhibition"
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
    read_All_textfile_write_csv('results/archives', "1993")

    
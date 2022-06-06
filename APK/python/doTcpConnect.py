from tkinter import E
from numpy import empty
from transformers import PreTrainedTokenizerFast

#set tokenizer
Q_TKN = "<usr>"
A_TKN = "<sys>"
BOS = '</s>'

EOS = '</s>'
MASK = '<unused0>'
SENT = '<unused1>'
PAD = '<pad>'

koGPT2_TOKENIZER = PreTrainedTokenizerFast.from_pretrained("skt/kogpt2-base-v2",
            bos_token=BOS, eos_token=EOS, unk_token='<unk>',
            pad_token=PAD, mask_token=MASK) 
print(koGPT2_TOKENIZER.tokenize("Tokenizer : Ready [],/."))

#set Model
import torch
from transformers import GPT2LMHeadModel
import os
path = 'models/_finalModel.bin'
if(os.path.exists(path)):
    model = torch.load(path)
elif(os.path.exists('python/' + path)) :
    model = torch.load('python/' + path)
else :
    exit(print("model not found"))
print('modelReady')

def RunGPT2Model(text) :
    sent = "0" # 0=일상, 1=부정, 2=긍정
    with torch.no_grad():
        q = text
        a = ""
        while 1:
            input_ids = torch.LongTensor(koGPT2_TOKENIZER.encode(Q_TKN + q + SENT + sent + A_TKN + a)).unsqueeze(dim=0)
            pred = model(input_ids)
            pred = pred.logits
            gen = koGPT2_TOKENIZER.convert_ids_to_tokens(torch.argmax(pred, dim=-1).squeeze().numpy().tolist())[-1]
            if gen == EOS:
                break
            a += gen.replace("▁", " ")
        print("Chatbot > {}".format(a.strip()))
        return a.strip()

#set TcpServer
import socket, threading;

isServerRunning = False 
def binder(client_socket, addr):
  global isServerRunning
  print('Connected by', addr)
  try:
    data = client_socket.recv(4)
    length = int.from_bytes(data, "big")
    data = client_socket.recv(length)
    msg = data.decode()
    print('Received from :', addr, msg)
    if len(msg) <= 1:
        isServerRunning = False
        client_socket.close()
        return
    msg = RunGPT2Model(msg)
    data = msg.encode()
    length = len(data)
    client_socket.sendall(length.to_bytes(4, byteorder='big'))
    client_socket.sendall(data)
  except Exception as e:
    print("binderDown by : ", e, "from : ", addr)
  finally:
    isServerRunning = False
    client_socket.close()

def Server(server_socket):
    global isServerRunning
    try:
        isServerRunning = True
        print("serverStart : ")
        client_socket, addr = server_socket.accept()
        th = threading.Thread(target=binder, args = (client_socket,addr))
        th.start()
    except Exception as e:
        print("serverDown by :", e)
    finally:
        print("serverDone : ")
#출처: https://nowonbun.tistory.com/670 [명월 일지:티스토리]

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind(('', 9999))
server_socket.listen()
try:
    while True:
        if isServerRunning == False:
            serverThread = threading.Thread(target=Server, args=(server_socket,)).start()
except KeyboardInterrupt :
    print("mainThreadStop : by KeyboardInterrupt")
except Exception as e:
    print("mainThreadDown by :", e)
finally:
    server_socket.close()
    serverThread.join()
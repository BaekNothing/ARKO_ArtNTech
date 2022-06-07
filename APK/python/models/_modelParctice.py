url = "https://colab.research.google.com/github/haven-jeon/KoGPT2-chatbot/blob/master/KoGPT2_chatbot_pytorch.ipynb"
url += "https://github.com/haven-jeon/KoGPT2-chatbot"
url += "https://github.com/SKT-AI/KoGPT2"
url += "https://vision-oasis.tistory.com/7?category=900851"
url += "https://github.com/ttop32/KoGPT2novel"

from transformers import PreTrainedTokenizerFast
tokenizer = PreTrainedTokenizerFast.from_pretrained("./kogpt2-base-v2/config.json",
  bos_token='</s>', eos_token='</s>', unk_token='<unk>',
  pad_token='<pad>', mask_token='<mask>')
print(tokenizer.tokenize("안녕하세요. 한국어 GPT-2 입니다.😤:)l^o"))

import torch
from transformers import GPT2LMHeadModel
import os

if(os.path.exists("models/kogpt2_model.bin")):
    model = torch.load("models/kogpt2_model.bin")
else:
    model = GPT2LMHeadModel.from_pretrained('skt/kogpt2-base-v2')
text = '나는 더 성장하고 싶어요. 천천히 생각해보자.'
input_ids = tokenizer.encode(text, return_tensors='pt')
gen_ids = model.generate(input_ids,
                           max_length=170,
                           repetition_penalty=2.0,
                           pad_token_id=tokenizer.pad_token_id,
                           eos_token_id=tokenizer.eos_token_id,
                           bos_token_id=tokenizer.bos_token_id,
                           use_cache=True)
generated = tokenizer.decode(gen_ids[0])
print(generated)
torch.save(model.state_dict(), 'models/kogpt2_state_dict.bin')
torch.save(model, 'models/kogpt2_model.bin')

#generate data.txt
# import pandas as pd
# df = pd.read_excel('kogptDataset/트위터 기반 대화 데이터셋/트위터_대화시나리오DB_2000Set.xlsx')
# df.to_csv('text_data.txt', index=False)

#https://github.com/ttop32/KoGPT2novel/blob/main/train.ipynb
import torch
import transformers
from transformers import AutoModelWithLMHead, PreTrainedTokenizerFast
from fastai.text.all import *
import fastai


import re

print(torch.__version__)
print(transformers.__version__)
print( fastai.__version__)

with open("data/text_data.txt", "r", -1, "utf-8") as f:
    lines = f.read()
lines=" ".join(lines.split())
len(lines)

lines=re.sub('\(계속\).*?[●○]', '', lines)
lines=re.sub('[●○]', '', lines)
len(lines)

#model input output tokenizer
class TransformersTokenizer(Transform):
    def __init__(self, tokenizer): self.tokenizer = tokenizer
    def encodes(self, x): 
        toks = self.tokenizer.tokenize(x)
        return tensor(self.tokenizer.convert_tokens_to_ids(toks))
    def decodes(self, x): return TitledStr(self.tokenizer.decode(x.cpu().numpy()))

#split data
train=lines[:int(len(lines)*0.9)]
test=lines[int(len(lines)*0.9):]
splits = [[0],[1]]

#init dataloader
tls = TfmdLists([train,test], TransformersTokenizer(tokenizer), splits=splits, dl_type=LMDataLoader)
batch,seq_len = 8,256
dls = tls.dataloaders(bs=batch, seq_len=seq_len)
# dls.show_batch(max_n=2)

#gpt2 ouput is tuple, we need just one val
class DropOutput(Callback):
    def after_pred(self): self.learn.pred = self.pred[0]
        
learn = Learner(dls, model, loss_func=CrossEntropyLossFlat(), cbs=[DropOutput], metrics=Perplexity()).to_fp16()
lr=learn.lr_find()
print(lr)
learn.fit_one_cycle(2, lr)
# learn.fine_tune(3)

import datetime as dt

torch.save(model.state_dict(), 'models/' + dt.datetime.now() + 'kogpt2_state_trained_dict.bin')
torch.save(model, 'models/' + dt.datetime.now() + 'kogpt2_model_trained.bin')

text = '나는 더 성장하고 싶어요. 천천히 생각해보자.'
input_ids = tokenizer.encode(text, return_tensors='pt')
gen_ids = model.generate(input_ids,
                           max_length=170,
                           repetition_penalty=2.0,
                           pad_token_id=tokenizer.pad_token_id,
                           eos_token_id=tokenizer.eos_token_id,
                           bos_token_id=tokenizer.bos_token_id,
                           use_cache=True)
generated = tokenizer.decode(gen_ids[0])
print(generated)
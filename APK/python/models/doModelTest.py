from numpy import empty
from transformers import PreTrainedTokenizerFast

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

import torch
from transformers import GPT2LMHeadModel
import os

path = input("Enter the path of the model you want to test : ")
if(os.path.exists(path)):
    model = torch.load(path)
else:
    exit(print("there is no model"))

text = " "
sent = "0" # 0=일상, 1=부정, 2=긍정
with torch.no_grad():
    while 1:
        q = input("user > ").strip()
        if q == "quit":
            break
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
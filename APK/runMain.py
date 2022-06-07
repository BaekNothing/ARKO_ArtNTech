import subprocess
import sys
import pip
import socket
import threading
import os

def install(package):
    subprocess.check_call([sys.executable, "-m", "pip", "install", package])

def pip_install_requirements(requirements_dir):
    subprocess.check_call([sys.executable, "-m", "pip", "install", "-r", requirements_dir.rstrip(".txt")+".txt"])

#check internet connection
if socket.gethostbyname(socket.gethostname())=="127.0.0.1":
    print("no Internet connection")
    exit()

#pip Update
try :
    subprocess.check_call([sys.executable, "-m", "pip", "install", "--upgrade", "--user", "pip"])
    pip_install_requirements('./requirements.txt')
except :
    print("error : pip update")
    exit(input())

def runUnity():
    try :
        subprocess.run(["./unity/ARKO_Unity.exe"])
    except Exception as e:
        print("error : unity ", e)
        exit(input())

#run unity.exe 
try :
    unityThread = threading.Thread(target=runUnity)
    unityThread.start()
except Exception as e:
    print("error : no unity.exe", e)
    exit(input())
    
#run doTcpNetwork.py
subprocess.run([sys.executable, "./python/doTcpConnect.py"])
os.kill(unityThread, 2)
exit(input())
import subprocess
import sys
import pip
import socket

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
    subprocess.check_call([sys.executable, "-m", "pip", "install", "--upgrade", "pip"])
    pip_install_requirements('./requirements.txt')
except :
    print("error : pip update")
    exit()

#run unity.exe 
try :
    subprocess.check_call(["./unity/ARKO_Unity.exe"])
except :
    print("error : no unity.exe")
    exit()
    
#run doTcpNetwork.py
try :
    subprocess.check_call([sys.executable, "./python/doTcpConnect.py"])
except :
    print("error : no doTcpNetwork.py")
    exit()


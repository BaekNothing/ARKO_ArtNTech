
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

class SocketManager : SingleTone<SocketManager>
{
    protected override void Init()
    {
        base.Init();
    }

    Thread thread;
    Socket client;
    string message;
    public void Send(string inputMessage)
    {
        message = inputMessage;
        if (message == null || message.Length <= 0)
            return;       
        thread = new Thread(WaitReceive);
        thread.Start();
        //출처: https://nowonbun.tistory.com/670 [명월 일지:티스토리]
    }

    string result = "";
    private void Update()
    {
        if (result != "")
        {
            DialogManager.instance.ShowPanel(new List<DialogManager.DialogInfoStruct> { new DialogManager.DialogInfoStruct(result, -1, "GPT2") });
            UIManager.instance.SetShowInputButtonInteractable(true);
            result = "";
        }
    }

    void WaitReceive()
    {
        result = "";
        using (client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            try
            {
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
                var data = Encoding.UTF8.GetBytes(message);
                client.Send(BitConverter.GetBytes(data.Length));
                client.Send(data);
                data = new byte[4];
                client.Receive(data, data.Length, SocketFlags.None);
                Array.Reverse(data);
                data = new byte[BitConverter.ToInt32(data, 0)];
                client.Receive(data, data.Length, SocketFlags.None);
                result = Encoding.UTF8.GetString(data);
            }
            catch (System.Exception e)
            {
                result = e.ToString();
            }
        }
    }
}
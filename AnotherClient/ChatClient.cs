using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AnotherClient
{
    class ChatClient
    {
        private TcpClient myclient;
        string incomingMessage;

        public ChatClient(TcpClient tcpClient)
        {
            this.myclient = tcpClient;
        }

        public (string, string) receiveMessage()
        {
            string sender;
            string message;
            NetworkStream networkStream = myclient.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            incomingMessage = streamReader.ReadLine();
            sender = incomingMessage.Split('~')[0];
            message = incomingMessage.Split('~')[1];
            Console.WriteLine("Message from " + sender + " : " + message);

            return (sender, message);
        }

        public bool sendMessage(string message, string receiver)
        {
            if (myclient.Connected)
            {
                NetworkStream networkStream = myclient.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                message = receiver + "~" + message;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

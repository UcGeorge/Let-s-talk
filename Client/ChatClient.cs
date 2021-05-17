using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
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

            using (NetworkStream networkStream = myclient.GetStream())
            using (StreamWriter streamWriter = new StreamWriter(networkStream))
            using (StreamReader streamReader = new StreamReader(networkStream))
            {
                incomingMessage = streamReader.ReadLine();
                streamWriter.WriteLine("Received");
                streamWriter.Flush();
                sender = incomingMessage.Split(":::")[0];
                message = incomingMessage.Split(":::")[1];
            }

            return (sender, message);
        }

        public bool sendMessage(string message, string receiver)
        {
            if (myclient.Connected)
            {

                using (NetworkStream networkStream = myclient.GetStream())
                using (StreamWriter streamWriter = new StreamWriter(networkStream))
                using (StreamReader streamReader = new StreamReader(networkStream))
                {
                    message = receiver + ":::" + message;
                    streamWriter.WriteLine(message);
                    streamWriter.Flush();
                    string response = streamReader.ReadLine();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

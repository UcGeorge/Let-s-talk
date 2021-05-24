using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
            Console.WriteLine("incoming: " + incomingMessage);
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
                message = receiver == "" ? message : receiver + "~" + message;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool sendFile(byte[] file, string receiver, string fileName)
        {
            if (myclient.Connected)
            {
                NetworkStream networkStream = myclient.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                streamWriter.WriteLine("File~"+receiver + "~" + fileName);
                streamWriter.Flush();

                int bufferSize = 1024;

                byte[] dataLength = BitConverter.GetBytes(file.Length);

                networkStream.Write(dataLength, 0, 4);

                int bytesSent = 0;
                int bytesLeft = file.Length;

                while (bytesLeft > 0)
                {
                    int curDataSize = Math.Min(bufferSize, bytesLeft);

                    networkStream.Write(file, bytesSent, curDataSize);

                    bytesSent += curDataSize;
                    bytesLeft -= curDataSize;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public byte[] receiveFile(string fileName, string sender)
        {
            byte[] data = new byte[0];

            using (TcpClient file_client = new TcpClient("localhost", 4321))
            using (NetworkStream stream = file_client.GetStream())
            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string message = sender + "~" + fileName;
                streamWriter.WriteLine(message);
                streamWriter.Flush();

                byte[] fileSizeBytes = new byte[4];
                int bytes = stream.Read(fileSizeBytes, 0, 4);
                int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

                int bytesLeft = dataLength;
                data = new byte[dataLength];

                int bufferSize = 1024;
                int bytesRead = 0;

                while (bytesLeft > 0)
                {
                    int curDataSize = Math.Min(bufferSize, bytesLeft);
                    if (file_client.Available < curDataSize)
                        curDataSize = file_client.Available;

                    bytes = stream.Read(data, bytesRead, curDataSize);

                    bytesRead += curDataSize;
                    bytesLeft -= curDataSize;
                    // Console.WriteLine("Bytes left: " + bytesLeft);
                }
            }

            return data;
        }

        public void Close()
        {
            NetworkStream stream = myclient.GetStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            StreamReader streamReader = new StreamReader(stream);

            //close all streams...
            streamReader.Close();
            streamWriter.Close();
            stream.Close();
        }
    }
}

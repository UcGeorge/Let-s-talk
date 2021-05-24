using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Shared;

namespace Server
{
    class ChatServer
    {
        private IPAddress iP;
        private int port;
        public static Dictionary<string, TcpClient> clientsList = new Dictionary<string, TcpClient>();
        public static Dictionary<string, Dictionary<string, ChatFile>> fileList = new Dictionary<string, Dictionary<string, ChatFile>>();
        TcpClient clientSocket;

        public ChatServer(IPAddress iPAddress, int port)
        {
            this.iP = iPAddress;
            this.port = port;
        }

        public void Start()
        {
            //TcpListener is listening on the given port...
            TcpListener tcpListener = new TcpListener(iP, port);
            tcpListener.Start();
            Console.WriteLine("Server Started");

            clientsList.Add("Admin", null);
            clientsList.Add("Broadcast", null);

            Thread fileServerThread = new Thread(fileServer);
            fileServerThread.Start();

            new Thread(() =>
            {
                while(true)
                {
                    sendContacts();
                    Thread.Sleep(500);
                }
            }).Start();

            while (true)
            {
                try
                {
                    //Accepts a new connection...
                    clientSocket = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Someone is trying to connect...");
                    handleClinet handleClinet = new handleClinet(clientSocket);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void fileServer()
        {
            //TcpListener is listening on the given port...
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 4321);
            tcpListener.Start();

            while (true)
            {
                try
                {
                    //Accepts a new connection...
                    TcpClient fileClientSocket = tcpListener.AcceptTcpClient();

                    new Thread(() => {
                        using (NetworkStream networkStream = fileClientSocket.GetStream())
                        using (StreamWriter streamWriter = new StreamWriter(networkStream))
                        using (StreamReader streamReader = new StreamReader(networkStream))
                        {
                            string initMessage = streamReader.ReadLine();
                            Console.WriteLine(String.Format("Client {0} is trying to get file: {1}", initMessage.Split("~")[0], initMessage.Split("~")[1]));

                            int bufferSize = 1024;

                            byte[] file = ChatServer.fileList[initMessage.Split("~")[0]][initMessage.Split("~")[1]].GetFile.Item2;

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
                        }
                    }).Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void sendContacts()
        {
            string message_list = "";
            foreach (KeyValuePair<string, TcpClient> i in clientsList)
            {
                if (i.Key == "Broadcast" || i.Key == "Admin" || i.Key == "Files" || i.Value.Connected)
                {
                    message_list += i.Key + "+";
                }
            }
            foreach (KeyValuePair<string, TcpClient> Item in clientsList)
            {
                if (Item.Key == "Broadcast" || Item.Key == "Admin" || Item.Key == "Files")
                {
                    continue;
                }
                if (Item.Value.Connected)
                {
                    NetworkStream networkStream = Item.Value.GetStream();
                    StreamWriter streamWriter = new StreamWriter(networkStream);
                    StreamReader streamReader = new StreamReader(networkStream);
                    string message = "Contacts~" + message_list.Trim('+');
                    streamWriter.WriteLine(message);
                    Console.WriteLine("To client - " + Item.Key + " : " + message);
                    streamWriter.Flush();
                }
            }
        }
    }

    public class handleClinet
    {
        string clName;
        TcpClient clientSocket;

        public handleClinet(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
            Thread clientThread = new Thread(startClient);
            clientThread.Start();
        }

        public void startClient()
        {
            using (NetworkStream networkStream = clientSocket.GetStream())
            using (StreamWriter streamWriter = new StreamWriter(networkStream))
            using (StreamReader streamReader = new StreamReader(networkStream))
            {
                clName = streamReader.ReadLine().Split("~")[1];
                string message = "Admin~Welcome, " + clName + "!";
                Console.WriteLine(clName + " just joined.");
                ChatServer.clientsList.Add(clName, clientSocket);
                ChatServer.fileList.Add(clName, new Dictionary<string, ChatFile>());
                broadcast(clName + " just joined.", "Admin");
                streamWriter.WriteLine(message);
                streamWriter.Flush();

                string incomingMessage = "";
                try
                {
                    while ((true))
                    {
                        incomingMessage = streamReader.ReadLine();
                        Console.WriteLine("From client - " + clName + " : " + incomingMessage);
                        string header = incomingMessage.Split("~")[0];

                        if (header == "Admin" || header == "Broadcast")
                        {
                            broadcast(incomingMessage.Split("~")[1], clName);
                        }
                        else if (header == "File")
                        {
                            string receiver = incomingMessage.Split("~")[1];
                            string fileName = incomingMessage.Split("~")[2];
                            try
                            {
                                byte[] file = receiveFile(networkStream);
                                ChatFile c = new ChatFile(fileName, file, clName);
                                ChatServer.fileList[receiver].Add(fileName, c);
                                sendTo(receiver, "", true, c);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                                streamWriter.WriteLine("Admin~An error occoured while receiving your file.");
                                streamWriter.Flush();
                            }
                        }
                        else if (header == "Close")
                        {
                            clientSocket.Close();
                            return;
                        }
                        else
                        {
                            sendTo(header, incomingMessage.Split("~")[1], false);
                        }
                    }//end while
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        byte[] receiveFile(NetworkStream stream)
        {
            byte[] fileSizeBytes = new byte[4];
            int bytes = stream.Read(fileSizeBytes, 0, 4);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            int bufferSize = 1024;
            int bytesRead = 0;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);
                if (clientSocket.Available < curDataSize)
                    curDataSize = clientSocket.Available;

                bytes = stream.Read(data, bytesRead, curDataSize);

                bytesRead += curDataSize;
                bytesLeft -= curDataSize;
            }

            return data;
        }

        public void broadcast(string msg, string uName)
        {
            foreach (KeyValuePair<string, TcpClient> kvp in ChatServer.clientsList)
            {
                if (kvp.Key == "Broadcast" || kvp.Key == "Admin" || kvp.Key == "Files")
                {
                    continue;
                }
                if (kvp.Value.Connected)
                {
                    NetworkStream networkStream = kvp.Value.GetStream();
                    StreamWriter streamWriter = new StreamWriter(networkStream);
                    StreamReader streamReader = new StreamReader(networkStream);
                    string message = uName + "~[BROADCAST] " + msg;
                    streamWriter.WriteLine(message);
                    Console.WriteLine("To client - " + kvp.Key + " : " + message);
                    streamWriter.Flush();
                }
            }
        }  //end broadcast function

        public void sendTo(string receiver, string msg, bool file, ChatFile filee = null)
        {
            if (ChatServer.clientsList[receiver].Connected)
            {
                NetworkStream networkStream = ChatServer.clientsList[receiver].GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                {
                    if (file)
                    {
                        string message = "File~";
                        message += (filee.Sender + "|" + filee.GetFile.Item1 + "|" + filee.GetFile.Item2.Length);
                        streamWriter.WriteLine(message);
                        Console.WriteLine("To client : " + message);
                        streamWriter.Flush();
                    }
                    else
                    {
                        string message = clName + "~" + msg;
                        streamWriter.WriteLine(message);
                        Console.WriteLine("To client : " + message);
                        streamWriter.Flush();
                    }
                }
            }
        }

    } //end class handleClinet
}

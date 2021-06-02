using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Server
{
    class ChatServer
    {
        private IPAddress iP;
        private int port;
        protected List<chat_client> onlineClients = new List<chat_client>();

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

            onlineClients.Add(chat_client.GetClient("Admin"));
            onlineClients.Add(chat_client.GetClient("Broadcast"));

            new Thread(fileServer).Start(); // Start file server

            new Thread(() =>
            {
                while (true)
                {
                    bool modified = false;
                    lock (onlineClients)
                    {
                        try
                        {
                            foreach (chat_client client in onlineClients)
                            {
                                if (client.Id != "Broadcast" && client.Id != "Admin" && !client.client.Connected)
                                {
                                    onlineClients.Remove(client);
                                    modified = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error while scanning connected clients\n{e.ToString()}");
                        }
                    }
                    if (modified)
                    {
                        sendContacts();
                        modified = false;
                    }

                    Thread.Sleep(10000);
                }
            }).Start(); // Thread to check for connected and disconnected clients

            while (true)
            {
                try
                {
                    //Accepts a new connection...
                    TcpClient clientSocket = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Someone is trying to connect...");
                    new Thread(() =>
                    {
                        HandleClient(clientSocket);
                    }).Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        void broadcast(string msg, string uName)
        {
            foreach (chat_client client in onlineClients)
            {
                if (client.Id != "Broadcast" && client.Id != "Admin" && client.client.Connected)
                {
                    NetworkStream networkStream = client.client.GetStream();
                    StreamWriter streamWriter = new StreamWriter(networkStream);
                    string message = uName + "~[BROADCAST] " + msg;
                    streamWriter.WriteLine(message);
                    Console.WriteLine("To client - " + client.Id + " : " + message);
                    streamWriter.Flush();
                }
            }
        }

        void SendTo(string receiver, string msg, string clName, chat_file filee = null)
        {
            chat_client receiverClient = onlineClients.Where(x => x.Id == receiver).FirstOrDefault();

            if (receiverClient.client.Connected)
            {
                NetworkStream networkStream = receiverClient.client.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                {
                    if (filee != null)
                    {
                        string message = "File~";
                        message += (filee.sender + "|" + filee.Id + "|" + filee.size);
                        streamWriter.WriteLine(message);
                        Console.WriteLine("To client : " + message);
                        streamWriter.Flush();
                    }
                    else
                    {
                        Console.WriteLine($"To be sent: {msg}.\nBy: {clName}\nTo: {receiver}");
                        string message = clName + '~' + msg;
                        streamWriter.WriteLine(message);
                        streamWriter.Flush();
                    }
                }
            }
        }

        byte[] receiveFile(NetworkStream stream, TcpClient clientSocket)
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

        void fileServer()
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

                    new Thread(() =>
                    {
                        using (NetworkStream networkStream = fileClientSocket.GetStream())
                        using (StreamWriter streamWriter = new StreamWriter(networkStream))
                        using (StreamReader streamReader = new StreamReader(networkStream))
                        {
                            string initMessage = streamReader.ReadLine();
                            Console.WriteLine(String.Format("Client {0} is trying to get file: {1}", initMessage.Split('~')[0], initMessage.Split('~')[1]));

                            int bufferSize = 1024;

                            var fileName = initMessage.Split('~')[1].Split('\\').Last();
                            var theFile = chat_file.GetFile(fileName);

                            byte[] file = File.ReadAllBytes(theFile.location);

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

        void sendContacts()
        {
            string message_list = "";
            foreach (chat_client client in onlineClients)
            {
                if (client.Id == "Broadcast" || client.Id == "Admin" || client.client.Connected)
                {
                    message_list += client.Id + "+";
                }
            }
            foreach (chat_client client in onlineClients)
            {
                if (client.Id != "Broadcast" && client.Id != "Admin" && client.client.Connected)
                {
                    SendTo(client.Id, message_list.Trim('+'), "Contacts");
                }
            }
        }

        void HandleClient(TcpClient clientSocket)
        {
            using (NetworkStream networkStream = clientSocket.GetStream())
            using (StreamWriter streamWriter = new StreamWriter(networkStream))
            using (StreamReader streamReader = new StreamReader(networkStream))
            {
                var name = streamReader.ReadLine().Split('~')[1]; // Get first message (username~password)

                string message = "Admin~Welcome, " + name + "!";
                Console.WriteLine(name + " just joined.");

                chat_client newClient = chat_client.GetClient(name, clientSocket);
                string clName = newClient.Id;
                onlineClients.Add(newClient); // Add new client to connected clients
                sendContacts(); // Send contacts to everybody

                streamWriter.WriteLine(message);
                streamWriter.Flush();

                string incomingMessage = "";
                try
                {
                    while ((true))
                    {
                        incomingMessage = streamReader.ReadLine();
                        Console.WriteLine("From client - " + newClient.Id + " : " + incomingMessage);
                        string header = incomingMessage.Split('~')[0];

                        if (header == "Admin" || header == "Broadcast")
                        {
                            broadcast(incomingMessage.Split('~')[1], newClient.Id);
                        }
                        else if (header == "File")
                        {
                            string receiver = incomingMessage.Split('~')[1];
                            string fileName = incomingMessage.Split('~')[2];
                            try
                            {
                                byte[] file = receiveFile(networkStream, clientSocket);
                                var fileLocation = $@"C:\Users\USER\OneDrive\Documents\LTFiles\{fileName.Split('\\').Last()}";
                                File.WriteAllBytes(fileLocation, file);
                                chat_file c = chat_file.GetFile(
                                    fileName, 
                                    fileLocation, 
                                    chat_client.GetClient(receiver), 
                                    newClient,
                                    file.Length);
                                using(var context = new Model1())
                                {
                                    context.chat_file.Add(c);
                                    context.SaveChanges();
                                }
                                SendTo(receiver, "", newClient.Id, c);
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
                            SendTo(header, incomingMessage.Split('~')[1], clName);
                        }
                    }//end while
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}

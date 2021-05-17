﻿using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading;

namespace Server
{
    class ChatServer
    {
        private IPAddress iP;
        private int port;
        public static Hashtable clientsList = new Hashtable();
        TcpClient clientSocket;

        public ChatServer(IPAddress iPAddress, int port)
        {
            this.iP = iPAddress;
            this.port = port;
        }

        [Obsolete]
        public void Start()
        {
            //TcpListener is listening on the given port...
            TcpListener tcpListener = new TcpListener(port);
            tcpListener.Start();
            Console.WriteLine("Server Started");

            while(true)
            {
                try
                {
                    //Accepts a new connection...
                    clientSocket = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Someone is trying to connect...");
                    handleClinet handleClinet = new handleClinet(clientSocket, clientsList);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public static void broadcast(string msg, string uName)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                NetworkStream networkStream = ((TcpClient)Item.Value).GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                string message = uName + "~" + msg;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                string response = streamReader.ReadLine();
            }
        }  //end broadcast function

        public static void sendTo(TcpClient receiver, string msg, string uName)
        {
            NetworkStream networkStream = receiver.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            {
                string message = uName + "~" + msg;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                string response = streamReader.ReadLine();
            }
        }
    }

    public class handleClinet
    {
        string clName;
        Hashtable clientsList;
        TcpClient clientSocket;

        public handleClinet(TcpClient clientSocket, Hashtable cList)
        {
            this.clientsList = cList;
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
                clientsList.Add(clName, clientSocket);
                streamWriter.WriteLine(message);
                streamWriter.Flush();

                string incomingMessage = "";
                try
                {
                    while ((true))
                    {
                        incomingMessage = streamReader.ReadLine();
                        Console.WriteLine("From client - " + clName + " : " + incomingMessage);

                        ChatServer.broadcast(incomingMessage, clName);
                    }//end while
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /*private void doChat(TcpClient clientSocket)
        {
            if (clientSocket.Connected)
            {
                string incomingMessage = "";
                using (NetworkStream networkStream = clientSocket.GetStream())
                using (StreamWriter streamWriter = new StreamWriter(networkStream))
                using (StreamReader streamReader = new StreamReader(networkStream))
                {
                    try
                    {
                        while ((true))
                        {
                            incomingMessage = streamReader.ReadLine();
                            streamWriter.WriteLine("Received");
                            streamWriter.Flush();
                            Console.WriteLine("From client - " + clName + " : " + incomingMessage);

                            ChatServer.broadcast(incomingMessage, clName);
                        }//end while
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine("Not connected!");
            }
        }//end doChat*/
    } //end class handleClinet
}

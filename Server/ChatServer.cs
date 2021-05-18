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
        public static Dictionary<string, TcpClient> clientsList = new Dictionary<string, TcpClient>();
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

            clientsList.Add("Admin", null);
            clientsList.Add("Broadcast", null);

            while(true)
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
                broadcast(clName + " just joined.", "Admin");
                sendContacts();
                streamWriter.WriteLine(message);
                streamWriter.Flush();

                string incomingMessage = "";
                try
                {
                    while ((true))
                    {
                        incomingMessage = streamReader.ReadLine();
                        Console.WriteLine("From client - " + clName + " : " + incomingMessage);
                        if (incomingMessage.Split("~")[0] == "Admin" || incomingMessage.Split("~")[0] == "Broadcast")
                        {
                            broadcast(incomingMessage.Split("~")[1], clName);
                            sendContacts();
                        }
                        else
                        {
                            sendTo(ChatServer.clientsList[incomingMessage.Split("~")[0]], incomingMessage.Split("~")[1]);
                            sendContacts();
                        }
                    }//end while
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void broadcast(string msg, string uName)
        {
            foreach (KeyValuePair<string, TcpClient> kvp in ChatServer.clientsList)
            {
                if (kvp.Key == "Broadcast" || kvp.Key == "Admin")
                {
                    continue;
                }
                if (kvp.Value.Connected)
                {
                    NetworkStream networkStream = kvp.Value.GetStream();
                    StreamWriter streamWriter = new StreamWriter(networkStream);
                    StreamReader streamReader = new StreamReader(networkStream);
                    string message = uName + "~" + msg;
                    streamWriter.WriteLine(message);
                    Console.WriteLine("To client - " + kvp.Key + " : " + message);
                    streamWriter.Flush();
                }
            }
        }  //end broadcast function

        public void sendTo(TcpClient receiver, string msg)
        {
            if (receiver.Connected)
            {
                NetworkStream networkStream = receiver.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                {
                    string message = clName + "~" + msg;
                    streamWriter.WriteLine(message);
                    Console.WriteLine("To client : " + message);
                    streamWriter.Flush();
                }
            }
        }

        public void sendContacts()
        {
            string message_list = "";
            foreach(KeyValuePair<string, TcpClient> i in ChatServer.clientsList)
            {
                if(i.Key == "Broadcast" || i.Key == "Admin" || i.Value.Connected)
                {
                    message_list += i.Key + "+";
                }
            }
            foreach (KeyValuePair<string, TcpClient> Item in ChatServer.clientsList)
            {
                if (Item.Key == "Broadcast" || Item.Key == "Admin")
                {
                    continue;
                }
                NetworkStream networkStream = Item.Value.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                string message = "Contacts~" + message_list.Trim('+');
                streamWriter.WriteLine(message);
                Console.WriteLine("To client - " + Item.Key + " : " + message);
                streamWriter.Flush();
            }
        }
    } //end class handleClinet
}

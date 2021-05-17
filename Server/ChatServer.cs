using System;
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
                        if (incomingMessage.Split("~")[0] == "Admin")
                        {
                            broadcast(incomingMessage.Split("~")[1], clName);
                        }
                        else
                        {
                            sendTo((TcpClient)clientsList[incomingMessage.Split("~")[0]], incomingMessage.Split("~")[1]);
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
            foreach (DictionaryEntry Item in clientsList)
            {
                NetworkStream networkStream = ((TcpClient)Item.Value).GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream);
                StreamReader streamReader = new StreamReader(networkStream);
                string message = uName + "~" + msg;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
            }
        }  //end broadcast function

        public void sendTo(TcpClient receiver, string msg)
        {
            NetworkStream networkStream = receiver.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            {
                string message = clName + "~" + msg;
                streamWriter.WriteLine(message);
                streamWriter.Flush();
            }
        }
    } //end class handleClinet
}

﻿using System;
using System.Net;

namespace Server
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            ChatServer chatServer = new ChatServer(IPAddress.Loopback, 1234);
            chatServer.Start();
        }
    }
}
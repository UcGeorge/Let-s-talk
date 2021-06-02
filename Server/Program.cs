using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            ChatServer chatServer = new ChatServer(IPAddress.Any, 1234);
            chatServer.Start();
        }
    }
}

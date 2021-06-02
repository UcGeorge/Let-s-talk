using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class chat_client
    {
        public TcpClient client;

        public static chat_client GetClient(string name, TcpClient client = null)
        {
            using (var ctx = new Model1())
            {
                IQueryable<chat_client> c = (from cl in ctx.chat_client
                                             where cl.Id == name
                                             select cl);
                if (c.Count() == 0)
                {
                    Console.WriteLine($"Client: {name} is new.");
                    chat_client me = new chat_client() 
                    { 
                        Id = name,
                        last_seen = DateTime.Now,
                        client = client,
                        date_added = DateTime.Now,
                    };
                    ctx.chat_client.Add(me);
                    ctx.SaveChanges();
                    return me;
                }
                else
                {
                    Console.WriteLine($"Client: {name} is not new.");
                    c.FirstOrDefault().last_seen = DateTime.Now;
                    c.FirstOrDefault().client = client;
                    ctx.SaveChanges();
                    return c.FirstOrDefault();
                }
            }
        }
    }

    public partial class chat_file
    {
        public static chat_file GetFile(string file_id, string location, chat_client receiver, chat_client sender, double size)
        {
            using (var ctx = new Model1())
            {
                IQueryable<chat_file> c = from f in ctx.chat_file
                                            where f.Id == file_id
                                            select f;
                if (c.Count() == 0)
                {
                    Console.WriteLine($"File: {file_id} is new.");
                    chat_file me = new chat_file()
                    {
                        Id = file_id,
                        location = location,
                        receiver = receiver,
                        sender = sender,
                        size = size,
                        dateadded = DateTime.Now,
                    };
                    ctx.chat_file.Add(me);
                    ctx.SaveChanges();
                    return me;
                }
                else
                {
                    Console.WriteLine($"File: {file_id} is not new.");
                    return c.FirstOrDefault();
                }
            }
        }

        public static chat_file GetFile(string file_id)
        {
            using (var ctx = new Model1())
            {
                IQueryable<chat_file> c = from f in ctx.chat_file
                                          where f.Id == file_id
                                          select f;
                if (c.Count() == 0)
                {
                    Console.WriteLine($"File: {file_id} is new. Returning null.");
                    return null;
                }
                else
                {
                    Console.WriteLine($"File: {file_id} is not new.");
                    return c.FirstOrDefault();
                }
            }
        }
    }
}

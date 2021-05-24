using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string clientName = Console.ReadLine();

            Form1 df = new Form1(clientName);
            Application.Run(df);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thong.Net;
namespace ClientTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.Connect("localhost", 10000);
            client.Start();
            Console.ReadKey();
        }
    }
}

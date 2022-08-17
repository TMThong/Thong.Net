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
            client.Handle = new handleMessageTest();
            client.Connect("localhost", 10000);
            client.Start();
            while (client.IsConnected)
            {
                String text = Console.ReadLine();
                Message ms = new Message(0);
                ms.Writer.Write(text);
                ms.Writer.Flush();
                client.SendMessage(ms);
            }
        }
        public class handleMessageTest : IHandleMessage
        {
            public void OnDisconected()
            {
                Console.WriteLine("Server Close");
            }

            public void OnHandle(Message message)
            {
                switch (message.Command)
                {
                    case 0:
                        {
                            String text = message.Reader.ReadString();
                            Console.WriteLine(text);
                            break;
                        }
                }
            }
        }
    }
}

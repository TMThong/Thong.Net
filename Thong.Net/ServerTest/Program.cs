using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thong.Net;
namespace ServerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server ser = new Server(10000);
            ServerHandleTest serverHandle = new ServerHandleTest();
            serverHandle.Server = ser;
            ser.ServerHandle = serverHandle;
            ser.Start();
            Console.ReadKey();
        }
    }
    public class ServerHandleTest : IServerHandle
    {
        public Server Server { get; set; }
        public void ClientDisconnected(Client client)
        {
            Console.WriteLine("Some client disconnected");
        }
        public void ClientConnected(Client client)
        {
            client.Handle = new handleMessageTest(Server);
        }
    }
    public class handleMessageTest : IHandleMessage
    {
        public Server Server;
        public handleMessageTest(Server s)
        {
            Server = s;
        }

        public void OnDisconected()
        {
           
        }

        public void OnHandle(Message message)
        {
            switch (message.Command)
            {
                case 0:
                    {
                        Server?.SendAll(message);
                        break;
                    }
            }
        }
    }
}

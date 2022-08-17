using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Thong.Net
{
    public class Server
    {
        public List<Client> Clients = new List<Client>();
        internal TcpListener Listener;
        public bool IsRunning { get; protected set; }
        public int Port { get; }
        public IServerHandle ServerHandle { get;  set; }
        public Server(int port)
        {
            Listener = new TcpListener(port);
            Port = port;
        }
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Listener.Start();
                Thread thread = new Thread(Start_);
                thread.IsBackground = true;
                thread.Start();
            }
            
        }
        void Start_()
        {
            while (IsRunning)
            {
                TcpClient client = Listener.AcceptTcpClient();
                Client client1 = new Client(this, client);
                client1.Start();
                Clients.Add(client1);
                ServerHandle?.ClientConnected(client1);
                Console.WriteLine("Some client connected");
            }
        }
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = true;
                Listener.Stop();
                foreach(var c in Clients.ToArray())
                {
                    c.Disconnect();
                }
            }
        }
        public void SendAll(Message message)
        {
            foreach(var c in Clients.ToArray())
            {
                c.SendMessage(message);
            }
        }
        public void SendAll(Message message , Client client)
        {
            foreach (var c in Clients.ToArray())
            {
                if(c != client)
                {
                    c.SendMessage(message);
                }
            }
        }

    }
}

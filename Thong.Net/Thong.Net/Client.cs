using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
namespace Thong.Net
{
    public class Client
    {
        internal Server server;
        public TcpClient TcpClient { get; protected set; }
        protected BinaryReader Reader { get; set; }
        protected BinaryWriter Writer { get; set; }
        private ArrayList Messages = new ArrayList();
        private Object LookObject = new object();
        public IHandleMessage Handle { get; set; }
        public bool IsConnected
        {
            get
            {
                if (TcpClient == null)
                {
                    return false;
                }
                return TcpClient.Connected;
            }
        }
        internal Client(Server server, TcpClient tcpClient)
        {
            this.server = server;
            TcpClient = tcpClient;
            initIO();
        }
        public Client()
        {

        }
        public void Connect(String host, int port)
        {
            TcpClient = new TcpClient();
            TcpClient.Connect(host, port);
            initIO();
        }

        internal void initIO()
        {
            if (TcpClient != null)
            {
                Reader = new BinaryReader(TcpClient.GetStream());
                Writer = new BinaryWriter(TcpClient.GetStream());
            }
        }
        public void Start()
        {
            Thread thread1 = new Thread(sendThread);
            thread1.IsBackground = true;
            thread1.Start();
            Thread thread2 = new Thread(readThread);
            thread2.IsBackground = true;
            thread2.Start();
        }
        void sendThread()
        {

            while (IsConnected && Writer != null)
            {
                try
                {
                    while (Messages.Count > 0)
                    {
                        Message m = (Message)Messages[0];
                        writeMessage(m);
                        Messages.RemoveAt(0);
                        Thread.Sleep(10);
                    }
                    lock (LookObject)
                    {
                        Monitor.Wait(LookObject);
                    }
                }
                catch(IOException ex)
                {
                    break;
                }
            }
            Messages.Clear();
            Disconnect();
        }
        void readThread()
        {


            while (IsConnected && Reader != null)
            {
                try
                {
                    Message m;
                    while ((m = readMessage()) != null)
                    {

                        Handle?.OnHandle(m);

                    }
                }
                catch (IOException ex)
                {
                    
                    break;
                }
            }

            Disconnect();
        }

        protected virtual void writeMessage(Message message)
        {

            Writer.Write(message.Command);
            byte[] buffer = message.Data;
            Writer.Write(buffer.Length);
            Writer.Write(buffer);
            Writer.Flush();
        }
        protected virtual Message readMessage()
        {
            byte command = Reader.ReadByte();
            byte[] buffer = Reader.ReadBytes(Reader.ReadInt32());
            return new Message(command, buffer);
        }
        public void SendMessage(Message m)
        {
            Messages.Add(m);
            lock (LookObject)
            {
                Monitor.Pulse(LookObject);
            }

        }
        public void Disconnect()
        {
            TcpClient?.Close();
            server?.Clients.Remove(this);
            server?.ServerHandle?.ClientDisconnected(this);
            TcpClient = null;
            Writer?.Close();
            Writer = null;
            Reader?.Close();
            Reader = null;
        }
    }
}

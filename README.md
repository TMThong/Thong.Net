
# Thong.Net

Simple TCP Server/Client



## Usage/Examples Server

```csharp
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
```

## Usage/Examples Client

```csharp
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
```

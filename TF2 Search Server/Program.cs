using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace TF2_Search_Server
{
    delegate void DelegateForReading(string data);

    class Program
    {
        static void Main(string[] args)
        {
            new Server();
            Console.ReadLine();
        }
    }

    class Server
    {
        TcpListener Listener;
        List<Client> Clients = new List<Client>();

        public Server()
        {
            Listener = new TcpListener(IPAddress.Any, 1337);
            Listener.Start();
            Console.WriteLine("ZHIV!");
            while (true)
            {
                Client NewClient = new Client();
                NewClient.TcpConnection = Listener.AcceptTcpClient();
                NewClient.NetStream = NewClient.TcpConnection.GetStream();
                NewClient.Connect(new DelegateForReading(OnRecieve));
            }
        }

        public void OnRecieve(string data)
        {
            Console.WriteLine("msg: " + data);
        }

        ~Server()
        {
            if (Listener != null)
            {
                Console.WriteLine("DED");
                Listener.Stop();
            }
        }


    }

    class Client
    {
        public TcpClient TcpConnection;
        public NetworkStream NetStream;
        private DelegateForReading Send;

        public void Connect(DelegateForReading arg)
        {
            Send = arg;
            Thread clientThread = new Thread(new ParameterizedThreadStart(ClientStream));
            clientThread.Start(this);
        }

        static void ClientStream(object arg)
        {
            Console.WriteLine("YA RODILSYA");
            Client ME = (Client)arg;

            byte[] recieve = new byte[64];

            while (true)
            {
                try
                {
                    ME.NetStream.Read(recieve, 0, 64);
                }
                catch
                {
                    Console.WriteLine("Client with ID:  has Disconnected!");
                    break;
                }
                ME.Send(" message: " + Encoding.ASCII.GetString(recieve));
                for (int i = 0; i < 64; i++)
                {
                    recieve[i] = 0;
                }
            }
        }
    }
}

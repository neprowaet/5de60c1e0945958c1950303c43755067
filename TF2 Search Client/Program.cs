using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Xml;


namespace TF2_Search_Client
{
    class Program
    {
        static TcpClient client;
        static NetworkStream netStream;
        static void Main(string[] args)
        {
            Connect("127.0.0.1", 1337);
        }

        static void Connect(string ip, int port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port); //IP с номером порта
            client = new TcpClient(); //подключение клиента
            XDocument xmlData = XDocument.Load("settings.xml");
            
            
            client.Connect(ipe); 
            netStream = client.GetStream();
            SendMessage(xmlData.Element("name").Value);
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));//получение данных
            receiveThread.Start();//старт потока
            Console.WriteLine("Connected!");
 
            while (true)
            {
                SendMessage();
            }
        }
 
        static void SendMessage()
        {
            Console.Write("Message: ");

            string message = ": " + Console.ReadLine();
            byte[] toSend = new byte[64];
            toSend = Encoding.ASCII.GetBytes(message);
            netStream.Write(toSend, 0, toSend.Length);
            netStream.Flush(); //удаление данных из потока
            //Console.WriteLine(message);
            for (int i = 0; i < message.Length; i++)
            {
                toSend[i] = 0;
            }
        }
        static void SendMessage(string msg)
        {
            string message = ": " + msg;
            byte[] toSend = new byte[64];
            toSend = Encoding.ASCII.GetBytes(message);
            netStream.Write(toSend, 0, toSend.Length);
            netStream.Flush(); //удаление данных из потока
            //Console.WriteLine(message);
            for (int i = 0; i < message.Length; i++)
            {
                toSend[i] = 0;
            }
        }
        
 
        static void ReceiveData(object e)
        {
            byte[] receiveMessage = new byte[64];
 
            while (true)
            {
                try
                {
                    netStream.Read(receiveMessage, 0, 64);//чтение сообщения
                }
                catch
                {
                    Console.WriteLine("The connection to the server was interrupted!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
 
                string message = Encoding.ASCII.GetString(receiveMessage);
                Console.WriteLine(message);//вывод сообщения
            }
        }
 
 
        static void Disconnect()
        {
            client.Close();//отключение клиента
            netStream.Close();//отключение потока
            Environment.Exit(0); //завершение процесса
        }
    }
}


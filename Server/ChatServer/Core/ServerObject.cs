using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatServer
{
    public class ServerObject
    {

        static TcpListener tcpListener; // сервер для прослушивания
        public List<ClientObject> clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(int id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }

        // прослушивание входящих подключений
        [Obsolete]
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(9090);

                tcpListener.Start();

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

      
        protected internal void BroadcastMessage(string message, int[] id_rec, string sType)
        {
            if (sType == "chat" || sType== "respounse")
            {


                for (int i = 0; i < clients.Count(); i++)
                {
                    if (clients[i].id == id_rec[0])
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(clients[i].Stream, Encoding.Unicode);
                        binaryWriter.Write(message);

                    }
                }
            }
            else
            {
                var arrRecipients = id_rec.Intersect(clients.Select(co => co.id)).ToArray();

                for (int i = 0; i < arrRecipients.Count(); i++)
                {
                    BinaryWriter binaryWriter = new BinaryWriter(clients[i].Stream, Encoding.Unicode);
                    binaryWriter.Write(message);
                }
            }

        }

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}
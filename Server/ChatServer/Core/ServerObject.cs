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

namespace ChatServer
{
    public class ServerObject
    {
        
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения
      
        //public Dictionary<int, string[]> countries = new Dictionary<int, string[]>(5);

        protected internal void AddConnection(ClientObject clientObject)
        {
             clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
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

                    ClientObject clientObject =  new ClientObject(tcpClient, this);
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

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id, Image image=null)
        {
            if(image!=null)
            {
                //BinaryFormatter formatter = new BinaryFormatter();

                var ms = new MemoryStream();
                    image.Save(ms, image.RawFormat);
                     byte[] data_ = ms.ToArray();
                    ms.Dispose();
                    ms.Close();
                using (var db = new  UserContext())
                {
                    var result = db.Users.SingleOrDefault(u => u.userId == 2);
                    if (result != null)
                    {
                        result.userImage = data_;
                        db.SaveChanges();
                    }
                }

                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].id == id) 
                    {
                        //image.Save(clients[i].Stream, image.RawFormat);
                        // formatter.Serialize(clients[i].Stream, image);
                        //clients[i].Stream.Write()

                        // BinaryWriter binaryWriter = new BinaryWriter(clients[i].Stream, Encoding.Unicode);

                        clients[i].Stream.Write(data_, 0, data_.Length); //передача данных

                    }
                }
            }
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].id == id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
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
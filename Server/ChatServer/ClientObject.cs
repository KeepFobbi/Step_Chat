using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
              


                // получаем имя пользователя
                string message = GetMessage();
                using (UserContext db = new UserContext())
                {
                    List<string> words = new List<string>();
                    string[] _words = message.Split(new char[] { ' ' });
                    foreach(var kek in _words)
                    {
                        words.Add(kek);
                    }
                    if (words[0] == "reg")
                    {
                        string temp = words[1];
                        var userId = from e in db.Users where e.userName == temp select e.userId;

                        var wow = userId.First();

                        userName = words[1];
                        server.BroadcastMessage(wow.ToString(), Id);

                    }
                    
                }


                /* message = GetMessage();
                 if (message == "1")
                 {
                     server.Rum_1 = new List<string>();
                     server.Rum_1.Add(this.Id);

                 }*/
              //  server.BroadcastMessage("ok", this.Id);
                message = userName + " вошел в чат";
                // посылаем сообщение о входе в чат всем подключенным пользователям
               
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        
                        message = GetMessage();
                       
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                   // Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
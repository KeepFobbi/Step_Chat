using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ChatServer.BD_Entities;

namespace ChatServer
{


    public class ClientObject
    {
        protected internal string id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера
        
        JSendAfterLogin jSendAfterLogin = new JSendAfterLogin();

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            var id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string message;


                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                 {
                     

                         message = GetMessage();

                        string[] _words = message.Split(' ');

                        if (_words[0] == "auth")
                        {
                            bool chek = Authorization(_words);
                            if (chek)
                            {
                                message = _words[1] + " вошел в чат";
                                Console.WriteLine(message);
                                server.BroadcastMessage(СompileResponseAfterLogin(), this.id);

                            }
                        }
                         if (_words[0] == "send")
                        {
                           // AfterReceiveMessage(_words);
                            server.BroadcastMessage(message, _words[1]);

                        }
                         if (_words[0] == "update")
                        {
                            AfterUpdateMessage(_words);
                            server.BroadcastMessage(message+$" {this.id}", _words[1]);
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
                server.RemoveConnection(this.id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[1024]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            string message;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                message = builder.ToString();
            }
            while (Stream.DataAvailable);




            return message;
        }

        private bool Authorization(string[] str)
        {
            using (UserContext db = new UserContext())
            {
                var _login = str[1];
                var _password = str[2];

                var query = from e in db.Users where (e.userName == _login && e.userPassword == _password) select e;


                var user = query.FirstOrDefault();

                jSendAfterLogin.User = user;

                if (user != null)
                {
                    this.id = user.userId.ToString();

                    return true;
                }
                else
                    return false;

            }
        }

        private string СompileResponseAfterLogin()
        {
            using (UserContext db = new UserContext())
            {
                int _id = Convert.ToInt32(this.id);

                var query_groups = from g in db.Groups
                                   join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                   where gm.userId == _id
                                   select g;


                var g_all = query_groups.ToList();

                /* var query_mess = from um in db.userMessages
                                  where um.senderId == _id 
                                  select um;*/

                jSendAfterLogin.Groups = g_all;

                string jsonData = JsonConvert.SerializeObject(jSendAfterLogin);

                return jsonData;

            }
        }

        private void AfterReceiveMessage(string[] content)
        {
            using (UserContext db = new UserContext())
            {
                userMessages userMessages = new userMessages();

                userMessages.senderId = Convert.ToInt32(this.id);
                userMessages.recipientId = Convert.ToInt32(content[1]);
                userMessages.createAt = Convert.ToDateTime(content[2]);
                userMessages.content = content[3];

                db.userMessages.Add(userMessages);
                db.SaveChanges();


            }
        }

        private void AfterUpdateMessage(string[] content)
        {
            using (UserContext db = new UserContext())
            {
                var updateTime = Convert.ToDateTime(content[3]);
                int mess_id = Convert.ToInt32(content[2]);

                var query = from um in db.userMessages
                            where um.messageId == mess_id
                            select um;

                foreach (userMessages userMessages in query)
                {
                    userMessages.content = content[4];
                }

                db.SaveChangesAsync();


            }
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
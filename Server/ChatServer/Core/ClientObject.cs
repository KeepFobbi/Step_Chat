using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ChatServer.BD_Entities;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ChatServer.Json_Classes;
using NJsonSchema;
using NJsonSchema.Validation;

namespace ChatServer
{

    public class ClientObject
    {
        protected internal string id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        TcpClient client;
        ServerObject server;
        UserContext db = new UserContext();
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

                while (true)
                {

                    message = GetMessage();

                    var loginSchemaFrame = NJsonSchema.JsonSchema.FromType<LoginEvent>();
                    var messageSchemaFrame = NJsonSchema.JsonSchema.FromType<MessageEvent>();
                    var openCorrSchemaFrame = NJsonSchema.JsonSchema.FromType<OpenCorrespondence>();

                    JSchema loginSchema = JSchema.Parse(loginSchemaFrame.ToJson().ToString());
                    JSchema messageSchema = JSchema.Parse(messageSchemaFrame.ToJson().ToString());
                    JSchema openCorrSchema = JSchema.Parse(openCorrSchemaFrame.ToJson().ToString());



                    if (JObject.Parse(message).IsValid(loginSchema))
                    {

                            LoginEvent loginEvent = new LoginEvent();
                            loginEvent = JsonConvert.DeserializeObject<LoginEvent>(message);

                            bool chek = Authorization(loginEvent);
                            if (chek)
                            {

                                Console.WriteLine(loginEvent.login + " вошел в чат");


                                //   Image image = Image.FromFile(@"D:\Archive\Archive\StepChat\StepChat\Step_Chat\Server\ChatServer\Photos\1200px-Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg");

                                server.BroadcastMessage(СompileResponseAfterLogin(), this.id);

                            }

                    }

                    if (JObject.Parse(message).IsValid(messageSchema))
                    {


                         MessageEvent messageEvent = JsonConvert.DeserializeObject<MessageEvent>(message);

                            MessagesEventsFunc(messageEvent);
                        
                    }

                    if (JObject.Parse(message).IsValid(openCorrSchema))
                    {

                        OpenCorrespondence openCorrespondence = JsonConvert.DeserializeObject<OpenCorrespondence>(message);

                        server.BroadcastMessage(AfterOpenChat(openCorrespondence), this.id);
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


        private string GetMessage()
        {
            byte[] data = new byte[16384]; 
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

        private string MessagesEventsFunc(MessageEvent messageEvent)
        {
            if (messageEvent.statusType == "Send")
            {

                int _id = Convert.ToInt32(this.id);

                var idrec = from u in db.Users
                            from pc in db.PrivateChats
                            where ((u.userId == pc.user_1_Id && (pc.user_1_Id != _id && pc.user_2_Id == _id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == _id && pc.user_2_Id != _id)) && pc.chatId == messageEvent.recipientIid

                            select u.userId;

                var rec = idrec.First();

                server.BroadcastMessage(SaveAfterReceiveMessage(messageEvent), rec.ToString());

                return "ok";

            }
            else if (messageEvent.statusType == "Delete")
            {
                for(int i=0;i<messageEvent.messages.Count();i++)
                {
                    int id_del = messageEvent.messages.Keys.ElementAt(i);
                    userMessages delMessage = db.userMessages.Where(um => um.messageId == id_del).FirstOrDefault();

                    db.userMessages.Remove(delMessage);
                }
                db.SaveChanges();
            }
            else if(messageEvent.statusType == "Update")
            {
                int id_update = messageEvent.messages.Keys.ElementAt(0);
                userMessages updateMessage = db.userMessages.Where(um => um.messageId == id_update).FirstOrDefault();

                db.SaveChanges();
            }
            else if (messageEvent.statusType == "ReSend")
            {

            }
                return "kk";

        }
        private bool Authorization(LoginEvent login)
        {

            var _login = login.login;
            var _password = login.password;

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

        private string СompileResponseAfterLogin()
        {

            int _id = Convert.ToInt32(this.id);


            var query_groups = from g in db.Groups
                               join gm in db.GroupsMembers on g.groupId equals gm.groupId
                               where gm.userId == _id
                               select g;


            var g_all = query_groups.ToList();

            var query_privateChats = from u in db.Users
                                     from pc in db.PrivateChats
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != _id && pc.user_2_Id == _id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == _id && pc.user_2_Id != _id))

                                     select pc;



            var q_pc = query_privateChats.ToList();

            var query_usersInChats = from u in db.Users
                                     from pc in db.PrivateChats
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != _id && pc.user_2_Id == _id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == _id && pc.user_2_Id != _id))

                                     select u;



            var q_uinc = query_usersInChats.ToList();



            var query_last_mess_g = from um in db.userMessages
                                    join mt in (
                                             ((from um_ in db.userMessages
                                               join g in db.Groups on um_.recipientGroupId equals g.groupId
                                               join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                               where um_.senderId == _id || (um_.recipientGroupId == g.groupId && gm.userId == _id)
                                               group um_ by new { um_.recipientGroupId } into g
                                               select new { g.Key.recipientGroupId, createAt = (DateTime)g.Max(p => p.createAt) }).Distinct()))
                                               on new { um.recipientGroupId, um.createAt } equals new { mt.recipientGroupId, mt.createAt }
                                    select um;

            var m_last = query_last_mess_g.ToList();

            var query_last_mess_c = from um in db.userMessages
                                    join mt in (
                                             ((from um_ in db.userMessages
                                               join pc in db.PrivateChats on um_.recipientChatId equals pc.chatId
                                               where (um_.senderId == _id || ((um_.recipientChatId == pc.chatId && (pc.user_1_Id == _id || pc.user_2_Id == _id))))
                                               group um_ by new { um_.recipientChatId } into g
                                               select new { g.Key.recipientChatId, createAt = (DateTime)g.Max(p => p.createAt) }).Distinct()))
                                               on new { um.recipientChatId, um.createAt } equals new { mt.recipientChatId, mt.createAt }
                                    select um;

            var m_last_c = query_last_mess_c.ToList();

            foreach (var mess in m_last_c)
            {
                m_last.Add(mess);
            }

            foreach (var kek in g_all)
            {
                StartInfo startInfo = new StartInfo
                {
                    groupId = kek.groupId,
                    groupName = kek.groupName,
                    content = m_last.Find(gr => gr.recipientGroupId == kek.groupId).content
                };

                jSendAfterLogin.startInfos.Add(startInfo);

            }

            foreach (var kek in q_pc)
            {
                StartInfo startInfo = new StartInfo
                {
                    chatId = kek.chatId,
                    userName = q_uinc.Find(u => u.userId == kek.user_1_Id || u.userId == kek.user_2_Id).userName,
                    content = m_last_c.Find(pc => pc.recipientChatId == kek.chatId).content
                };

                jSendAfterLogin.startInfos.Add(startInfo);

            }

            string jsonData = JsonConvert.SerializeObject(jSendAfterLogin, Formatting.Indented);

            Console.WriteLine(jsonData);

            return jsonData;


        }

        private string SaveAfterReceiveMessage(MessageEvent messageEvent)
        {

            List<userMessages> userMessage = new List<userMessages>();
            string jsonData;

            if (messageEvent.recipientTtype == "chat")
            {
                userMessage.Add(new userMessages
                {
                    recipientChatId = messageEvent.recipientIid,
                    createAt = messageEvent.eventTime,
                    senderId = Convert.ToInt32(this.id),
                    content = messageEvent.messages[-1]
                }); 

                jsonData = "chat" + JsonConvert.SerializeObject(userMessage, Formatting.Indented);
                Console.WriteLine(jsonData);
                db.userMessages.Add(userMessage[0]);
                db.SaveChanges();
                return jsonData;
            }
            else 
            {
                userMessage.Add(new userMessages
                {
                    recipientGroupId = messageEvent.recipientIid,
                    createAt = messageEvent.eventTime,
                    senderId = Convert.ToInt32(this.id),
                    content = messageEvent.messages[-1]
                }) ;

                jsonData = "group" + JsonConvert.SerializeObject(userMessage, Formatting.Indented);
                Console.WriteLine(jsonData);
                db.userMessages.Add(userMessage[0]);
                db.SaveChanges();
                return jsonData;
            }

        }

        private string AfterOpenChat(OpenCorrespondence openCorr)
        {

            int _id = Convert.ToInt32(this.id);

            if (openCorr.statusType=="chat")
            {
                var query_mess_c = from um in db.userMessages
                                   join pc in db.PrivateChats on um.recipientChatId equals pc.chatId
                                   where (um.recipientChatId == openCorr.idCorr)
                                   select um;
                var qm = query_mess_c.OrderBy(q => q.createAt).ToList();

                List<userMessages> userMessages = new List<userMessages>();
                userMessages = qm;

                string jsonData = "chat" + JsonConvert.SerializeObject(userMessages, Formatting.Indented);

                Console.WriteLine(jsonData);

                return jsonData;
            }
            else
            {

                var query_mess_g = from um in db.userMessages
                                   where um.recipientGroupId == openCorr.idCorr
                                   select um;
                var qm = query_mess_g.OrderBy(q => q.createAt).ToList();
                List<userMessages> userMessages = new List<userMessages>();
                userMessages = qm;

                string jsonData = "group" + JsonConvert.SerializeObject(userMessages, Formatting.Indented);

                Console.WriteLine(jsonData);

                return jsonData;
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
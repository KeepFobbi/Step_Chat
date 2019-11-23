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
        protected internal int id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        TcpClient client;
        ServerObject server;
        UserContext db = new UserContext();
        JSendAfterLogin jSendAfterLogin = new JSendAfterLogin();

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            id = 0;
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

                    var message_Json = JObject.Parse(message);

                    Console.WriteLine(message_Json);

                    if (message_Json.IsValid(loginSchema))
                    {

                        LoginEvent loginEvent = new LoginEvent();
                        loginEvent = JsonConvert.DeserializeObject<LoginEvent>(message_Json.ToString());

                        bool chek = Authorization(loginEvent);

                        if (chek)
                        {
                            Console.WriteLine(loginEvent.login + " вошел в чат");

                            foreach (var client in server.clients)
                            {
                                Console.WriteLine(client.id + "-- idUser");
                            }

                            server.BroadcastMessage(СompileResponseAfterLogin(), new int[1] { id }, "resounse");
                        }
                    }
                    if (message_Json.IsValid(messageSchema))
                    {
                        MessageEvent messageEvent = JsonConvert.DeserializeObject<MessageEvent>(message_Json.ToString());

                        MessagesEventsFunc(messageEvent);
                    }

                    if (message_Json.IsValid(openCorrSchema))
                    {

                        OpenCorrespondence openCorrespondence = JsonConvert.DeserializeObject<OpenCorrespondence>(message_Json.ToString());
                        server.BroadcastMessage(AfterOpenChat(openCorrespondence), new int[1] { id }, "resounse");
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
            StringBuilder builder = new StringBuilder();
            BinaryReader binaryReader = new BinaryReader(Stream, Encoding.Unicode);
            string message;
            builder.Append(binaryReader.ReadString());
            message = builder.ToString();
            return message;
        }

        private void MessagesEventsFunc(MessageEvent messageEvent)
        {
            if (messageEvent.statusType == "Send")
            {
                SendMessages(messageEvent);
            }
            else if (messageEvent.statusType == "Delete")
            {
                DeleteMessage(messageEvent);
            }
            else if (messageEvent.statusType == "Update")
            {
                UpdateMessage(messageEvent);
            }
        }

        private void SendMessages(MessageEvent messageEvent)
        {

            if (messageEvent.recipientTtype == "chat")
            {
                PrivateChats privateChatSend = db.PrivateChats.Where(pc => pc.chatId == messageEvent.recipientId).FirstOrDefault();

                int id_rec = privateChatSend.user_1_Id == id ? privateChatSend.user_2_Id : privateChatSend.user_1_Id;

                server.BroadcastMessage(SaveAfterReceiveMessage(messageEvent), new int[1] { id_rec }, "chat");
            }

            else if (messageEvent.recipientTtype == "group")
            {
                server.BroadcastMessage(SaveAfterReceiveMessage(messageEvent), GetArrayIdUserInGroup(), "group");
            }
        }

        private string SaveAfterReceiveMessage(MessageEvent messageEvent)
        {
            userMessages userMessage = new userMessages();
            userMessagesList userMessagesList = new userMessagesList();

            if (messageEvent.recipientTtype == "chat")
            {
                userMessage.recipientChatId = messageEvent.recipientId;
                userMessage.createAt = messageEvent.eventTime;
                userMessage.senderId = id;
                userMessage.content = messageEvent.messages[-1];

                userMessagesList.uMList.Add(userMessage);

                db.userMessages.Add(userMessage);
                db.SaveChanges();

                string jsonData = JsonConvert.SerializeObject(userMessagesList, Formatting.Indented);

                Console.WriteLine(jsonData);

                return jsonData;
            }
            else if (messageEvent.recipientTtype == "group")
            {
                userMessage.recipientGroupId = messageEvent.recipientId;
                userMessage.createAt = messageEvent.eventTime;
                userMessage.senderId = id;
                userMessage.content = messageEvent.messages[-1];

                userMessagesList.uMList.Add(userMessage);

                db.userMessages.Add(userMessage);
                db.SaveChanges();

                string jsonData = JsonConvert.SerializeObject(userMessagesList, Formatting.Indented);

                Console.WriteLine(jsonData);

                return jsonData;
            }
            else
            {
                return "MessageEvent type error";
            }
        }

        private void DeleteMessage(MessageEvent messageEvent)
        {
            int id_del = messageEvent.messages.Keys.FirstOrDefault();
            userMessages delMessage = db.userMessages.Where(um => um.messageId == id_del).FirstOrDefault();
            db.userMessages.Remove(delMessage);
            db.SaveChanges();

            if (messageEvent.recipientTtype == "chat")
            {
                PrivateChats privateChatSend = db.PrivateChats.Where(pc => pc.chatId == messageEvent.recipientId).FirstOrDefault();

                int id_rec = privateChatSend.user_1_Id == id ? privateChatSend.user_2_Id : privateChatSend.user_1_Id;

                MessageEvent mEvent = new MessageEvent("DeleteRespounse", messageEvent.recipientTtype, messageEvent.recipientId.ToString(), messageEvent.eventTime, id_del, String.Empty);

                var jsonDeleted = JsonConvert.SerializeObject(mEvent, Formatting.Indented);

                server.BroadcastMessage(jsonDeleted, new int[1] { id_rec }, "chat");
            }
            else if (messageEvent.recipientTtype == "group")
            {

                MessageEvent mEvent = new MessageEvent("DeleteRespounse", messageEvent.recipientTtype, messageEvent.recipientId.ToString(), messageEvent.eventTime, id_del, String.Empty);

                var jsonDeleted = JsonConvert.SerializeObject(mEvent, Formatting.Indented);

                server.BroadcastMessage(jsonDeleted, GetArrayIdUserInGroup(), "group");
            }
        }

        private void UpdateMessage(MessageEvent messageEvent)
        {
            int id_update = messageEvent.messages.Keys.ElementAt(0);
            userMessages updateMessage = db.userMessages.Where(um => um.messageId == id_update).FirstOrDefault();
            updateMessage.content = messageEvent.messages.Values.ElementAt(0);
            updateMessage.updateAt = messageEvent.eventTime;
            db.SaveChanges();

            if (messageEvent.recipientTtype == "chat")
            {
                PrivateChats privateChatSend = db.PrivateChats.Where(pc => pc.chatId == messageEvent.recipientId).FirstOrDefault();

                int id_rec = privateChatSend.user_1_Id == id ? privateChatSend.user_2_Id : privateChatSend.user_1_Id;

                MessageEvent mEvent = new MessageEvent("UpdateRespounse", messageEvent.recipientTtype, messageEvent.recipientId.ToString(), messageEvent.eventTime, id_update, updateMessage.content);

                var jsonUpdated = JsonConvert.SerializeObject(mEvent, Formatting.Indented);

                server.BroadcastMessage(jsonUpdated, new int[1] { id_rec }, "chat");
            }
            else if (messageEvent.recipientTtype == "group")
            {
                MessageEvent mEvent = new MessageEvent("UpdateRespounse", messageEvent.recipientTtype, messageEvent.recipientId.ToString(), messageEvent.eventTime, id_update, updateMessage.content);

                var jsonUpdated = JsonConvert.SerializeObject(mEvent, Formatting.Indented);

                server.BroadcastMessage(jsonUpdated, GetArrayIdUserInGroup(), "group");
            }

        }

        private int[] GetArrayIdUserInGroup()
        {
            var query_groupsMembers = from g in db.Groups
                                      join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                      where gm.userId == id
                                      select gm;

            var query_usersInGroups = from u in db.Users
                                      from gm in db.GroupsMembers
                                      from q_gml in query_groupsMembers
                                      where (gm.userId == u.userId && gm.groupId == q_gml.groupId)

                                      select u;

            var q_usersInGroupList = query_usersInGroups.Distinct().ToList();

            int[] array_id_user_in_group = new int[q_usersInGroupList.Count()];

            for (int i = 0; i < array_id_user_in_group.Count(); i++)
            {
                array_id_user_in_group[i] = q_usersInGroupList[i].userId;
            }

            return array_id_user_in_group;
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
                id = user.userId;

                return true;
            }
            else
            {
                return false;
            }

        }

        private string СompileResponseAfterLogin()
        {
            var query_groupsMembers = from g in db.Groups
                                      join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                      where gm.userId == id
                                      select gm;

            var q_groupsMembersList = query_groupsMembers.Distinct().ToList();

            var query_privateChats = from u in db.Users
                                     from pc in db.PrivateChats
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != id && pc.user_2_Id == id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == id && pc.user_2_Id != id))

                                     select pc;

            var q_privateChatsList = query_privateChats.Distinct().ToList();


            var query_usersInGroups = from u in db.Users
                                      from gm in db.GroupsMembers
                                      from q_gml in query_groupsMembers
                                      where (gm.userId == u.userId && gm.groupId == q_gml.groupId)

                                      select u;

            var q_usersInGroupList = query_usersInGroups.Distinct().ToList();

            var query_usersInChats = from u in db.Users
                                     from pc in db.PrivateChats
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != id && pc.user_2_Id == id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == id && pc.user_2_Id != id))

                                     select u;

            var q_userInChatsList = query_usersInChats.Distinct().ToList();

            var q_last_mess_g = from um in db.userMessages
                                join mt in (
                                         ((from um_ in db.userMessages
                                           join g in db.Groups on um_.recipientGroupId equals g.groupId
                                           join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                           where um_.senderId == id || (um_.recipientGroupId == g.groupId && gm.userId == id)
                                           group um_ by new { um_.recipientGroupId } into g
                                           select new { g.Key.recipientGroupId, createAt = (DateTime)g.Max(p => p.createAt) }).Distinct()))
                                           on new { um.recipientGroupId, um.createAt } equals new { mt.recipientGroupId, mt.createAt }
                                select um;

            var q_lastMessageJoint = q_last_mess_g.Distinct().ToList();

            var query_last_mess_c = from um in db.userMessages
                                    join mt in (
                                             ((from um_ in db.userMessages
                                               join pc in db.PrivateChats on um_.recipientChatId equals pc.chatId
                                               where (um_.senderId == id || ((um_.recipientChatId == pc.chatId && (pc.user_1_Id == id || pc.user_2_Id == id))))
                                               group um_ by new { um_.recipientChatId } into g
                                               select new { g.Key.recipientChatId, createAt = (DateTime)g.Max(p => p.createAt) }).Distinct()))
                                               on new { um.recipientChatId, um.createAt } equals new { mt.recipientChatId, mt.createAt }
                                    select um;


            var q_lastMessagesInChatsList = query_last_mess_c.Distinct().ToList();

            for (int i = 0; i < q_privateChatsList.Count(); i++)
            {
                if (q_lastMessagesInChatsList.Count() != q_privateChatsList.Count())
                {
                    q_lastMessagesInChatsList.Add(new userMessages { content = "", recipientChatId = q_privateChatsList[i].chatId });
                }

            }

            foreach (var mess in q_lastMessagesInChatsList)
            {
                q_lastMessageJoint.Add(mess);
            }

            var query_groups = from g in db.Groups

                               select g;

            var q_groupsList = query_groups.Distinct().ToList();

            foreach (var groupM in q_groupsMembersList)
            {
                StartInfo startInfo = new StartInfo
                {
                    groupId = groupM.groupId,
                    groupName = q_groupsList.Find(g => g.groupId == groupM.groupId).groupName,
                    userName = q_usersInGroupList.Find(u => u.userId == q_lastMessageJoint.Find(um => um.senderId == groupM.userId).senderId).userName,
                    content = q_lastMessageJoint.Find(m => m.recipientGroupId == groupM.groupId).content
                };

                jSendAfterLogin.startInfos.Add(startInfo);

            }

            foreach (var chat in q_privateChatsList)
            {
                StartInfo startInfo = new StartInfo
                {
                    chatId = chat.chatId,
                    //groupName = q_userInChatsList.Find(u => u.userId == (chat.user_1_Id == (q_lastMessagesInChatsList.Find(m=>m.recipientChatId==chat.chatId).senderId) ? chat.user_2_Id : chat.user_1_Id)).userName,
                    userName = q_userInChatsList.Find(u => u.userId == (chat.user_1_Id == id ? chat.user_2_Id : chat.user_1_Id)).userName,
                    content = q_lastMessagesInChatsList.Find(m => m.recipientChatId == chat.chatId).content
                };

                jSendAfterLogin.startInfos.Add(startInfo);

            }

            string jsonData = JsonConvert.SerializeObject(jSendAfterLogin, Formatting.Indented);

            Console.WriteLine(jsonData);

            return jsonData.ToString();
        }

        private string AfterOpenChat(OpenCorrespondence openCorr)
        {
            userMessagesList uMessList = new userMessagesList();

            if (openCorr.statusType == "chat")
            {
                var query_mess_c = from um in db.userMessages
                                   join pc in db.PrivateChats on um.recipientChatId equals pc.chatId
                                   where (um.recipientChatId == openCorr.idCorr)
                                   select um;
                var qm = query_mess_c.OrderBy(q => q.createAt).ToList();

                foreach (var mess in qm)
                {
                    uMessList.uMList.Add(mess);
                }

                string jsonData = JsonConvert.SerializeObject(uMessList, Formatting.Indented);

                Console.WriteLine(jsonData);

                return jsonData;
            }
            else
            {
                var query_mess_g = from um in db.userMessages
                                   where um.recipientGroupId == openCorr.idCorr
                                   select um;
                var qm = query_mess_g.OrderBy(q => q.createAt).ToList();

                foreach (var mess in qm)
                {
                    uMessList.uMList.Add(mess);
                }

                string jsonData = JsonConvert.SerializeObject(uMessList, Formatting.Indented);

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
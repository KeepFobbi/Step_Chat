﻿using Newtonsoft.Json;
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
                            //   Image image = Image.FromFile(@"D:\Archive\Archive\StepChat\StepChat\Step_Chat\Server\ChatServer\Photos\1200px-Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg");
                            // int[] ids_rec = new int[1];
                            int ids_rec = id;

                            server.BroadcastMessage(СompileResponseAfterLogin(), ids_rec);

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
                        //int[] ids_rec = new int[1];
                        int ids_rec = id;
                        server.BroadcastMessage(AfterOpenChat(openCorrespondence), ids_rec);
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
          //  byte[] data = new byte[10000000];
            StringBuilder builder = new StringBuilder();
            BinaryReader binaryReader = new BinaryReader(Stream, Encoding.Unicode);
            // int bytes = 0;
            string message;
            //do
            //{
            //    bytes = Stream.Read(data, 0, data.Length);
            //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            //    message = builder.ToString();
            //}
            //while (Stream.DataAvailable);

            builder.Append(binaryReader.ReadString());
            message = builder.ToString();
            return message;
        }

        private string MessagesEventsFunc(MessageEvent messageEvent)
        {
            if (messageEvent.statusType == "Send")
            {
                SendMessages(messageEvent);
            }
            else if (messageEvent.statusType == "Delete")
            {
                List<MessageEvent> listDeleted = new List<MessageEvent>();
                // int[] ids_del = new int[messageEvent.messages.Count()];
                int id_del = messageEvent.messages.Keys.FirstOrDefault();

                //for (int i = 0; i < messageEvent.messages.Count(); i++)messageEvent.messages[messageEvent.messages.Keys.FirstOrDefault()]
                //{

                // ids_del[i] = id_del;

                userMessages delMessage = db.userMessages.Where(um => um.messageId == id_del).FirstOrDefault();
                //"sendRespounse", messageEvent.recipientTtype, messageEvent.recipientIid.ToString(), messageEvent.eventTime, -1, messageEvent.messages[-1]
                MessageEvent mEvent = new MessageEvent("sendRespounse", messageEvent.recipientTtype, messageEvent.recipientIid.ToString(), messageEvent.eventTime, -2, messageEvent.messages[messageEvent.messages.Keys.FirstOrDefault()]);
                //listDeleted.Add(mEvent);


                db.userMessages.Remove(delMessage);
                // }
                var jsonDeleted = JsonConvert.SerializeObject(listDeleted, Formatting.Indented);

                //server.BroadcastMessage(jsonDeleted, id);
                db.SaveChanges();
            }
            else if (messageEvent.statusType == "Update")
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

        private void SendMessages(MessageEvent messageEvent)
        {
            int _id = Convert.ToInt32(this.id);


            if (messageEvent.recipientTtype == "chat")
            {
                PrivateChats privateChatSend = db.PrivateChats.Where(pc => pc.chatId == messageEvent.recipientIid).FirstOrDefault();

                //int[] ids_rec = new int[1];
                int ids_rec = privateChatSend.user_1_Id == _id ? privateChatSend.user_2_Id : privateChatSend.user_1_Id;

                server.BroadcastMessage(SaveAfterReceiveMessage(messageEvent), ids_rec);
            }

            //else if (messageEvent.recipientTtype == "group")
            //{
            //    GroupsMembers groupsMemberSend = db.GroupsMembers.Where(gm => gm.groupId == messageEvent.recipientIid).FirstOrDefault();

            //    var userArr = from u in db.Users
            //                  join gm in db.GroupsMembers on u.userId equals gm.userId
            //                  where gm.groupId == messageEvent.recipientIid && gm.userId != _id
            //                  select u.userId;

            //    int[] ids_rec = new int[userArr.Count()];

            //    for (int i = 0; i < userArr.Count(); i++)
            //    {
            //        ids_rec[i] = userArr.ElementAt(i);
            //    }

            //    server.BroadcastMessage(SaveAfterReceiveMessage(messageEvent), ids_rec);

            //}
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
                return false;

        }

        private string СompileResponseAfterLogin()
        {

            int _id = id;

            var query_groupsMembers = from g in db.Groups
                                      join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                      where gm.userId == _id
                                      select gm;

            var q_groupsMembersList = query_groupsMembers.Distinct().ToList();

            var query_privateChats = from u in db.Users
                                     from pc in db.PrivateChats
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != _id && pc.user_2_Id == _id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == _id && pc.user_2_Id != _id))

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
                                     where ((u.userId == pc.user_1_Id && (pc.user_1_Id != _id && pc.user_2_Id == _id)) || (u.userId == pc.user_2_Id) && (pc.user_1_Id == _id && pc.user_2_Id != _id))

                                     select u;

            var q_userInChatsList = query_usersInChats.Distinct().ToList();

            var q_last_mess_g = from um in db.userMessages
                                join mt in (
                                         ((from um_ in db.userMessages
                                           join g in db.Groups on um_.recipientGroupId equals g.groupId
                                           join gm in db.GroupsMembers on g.groupId equals gm.groupId
                                           where um_.senderId == _id || (um_.recipientGroupId == g.groupId && gm.userId == _id)
                                           group um_ by new { um_.recipientGroupId } into g
                                           select new { g.Key.recipientGroupId, createAt = (DateTime)g.Max(p => p.createAt) }).Distinct()))
                                           on new { um.recipientGroupId, um.createAt } equals new { mt.recipientGroupId, mt.createAt }
                                select um;

            var q_lastMessageJoint = q_last_mess_g.Distinct().ToList();

            var query_last_mess_c = from um in db.userMessages
                                    join mt in (
                                             ((from um_ in db.userMessages
                                               join pc in db.PrivateChats on um_.recipientChatId equals pc.chatId
                                               where (um_.senderId == _id || ((um_.recipientChatId == pc.chatId && (pc.user_1_Id == _id || pc.user_2_Id == _id))))
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
                    //  groupName = q_userInChatsList.Find(u => u.userId == (chat.user_1_Id == (q_lastMessagesInChatsList.Find(m=>m.recipientChatId==chat.chatId).senderId) ? chat.user_2_Id : chat.user_1_Id)).userName,
                    userName = q_userInChatsList.Find(u => u.userId == (chat.user_1_Id == _id ? chat.user_2_Id : chat.user_1_Id)).userName,
                    content = q_lastMessagesInChatsList.Find(m => m.recipientChatId == chat.chatId).content
                };

                jSendAfterLogin.startInfos.Add(startInfo);

            }

            string jsonData = JsonConvert.SerializeObject(jSendAfterLogin, Formatting.Indented);

            Console.WriteLine(jsonData);

            return jsonData.ToString();
        }

        private string SaveAfterReceiveMessage(MessageEvent messageEvent)
        {
            userMessages userMessage = new userMessages();
           // MessageEvent mEvent = new MessageEvent("SendRespounse", messageEvent.recipientTtype, messageEvent.recipientIid.ToString(), messageEvent.eventTime, -1, messageEvent.messages[-1]);
            userMessagesList userMessagesList = new userMessagesList();

            if (messageEvent.recipientTtype == "chat")
            {


                userMessage.recipientChatId = messageEvent.recipientIid;
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

                userMessage = new userMessages
                {
                    recipientGroupId = messageEvent.recipientIid,
                    createAt = messageEvent.eventTime,
                    senderId = Convert.ToInt32(this.id),
                    content = messageEvent.messages[-1]
                };



                string jsonData = JsonConvert.SerializeObject(userMessage, Formatting.Indented);


                Console.WriteLine(jsonData);
                db.userMessages.Add(userMessage);
                db.SaveChanges();

                return jsonData;
            }

        }

        private string AfterOpenChat(OpenCorrespondence openCorr)
        {

            int _id = Convert.ToInt32(this.id);
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
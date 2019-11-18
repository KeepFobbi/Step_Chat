using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NJsonSchema;
using Newtonsoft.Json.Linq;

namespace Chat
{
    public static class ConnectToServer
    {
        //37.115.128.11  178.92.84.69
        private const string host = "127.0.0.1";
        //private const string host = "37.115.128.11";
        private const int port = 9090;
        private const int V = 500000;
        static TcpClient client;
        static NetworkStream stream;

        public delegate void receiveLogin(JSendAfterLogin mess);
        public static event receiveLogin receiveLoginEv;
        public delegate void UserMess(MessageEvent mess, bool totalFlag);
        public static event UserMess UserMessEvent;
        public delegate void UserMessList(userMessagesList mess, bool totalFlag);
        public static event UserMessList UserMessListItem;
        public delegate void SystemError(bool Connect);
        public static event SystemError SystemErrorConnectToServer;

        public static string loginToServer { get; set; }
        public static string passwordToServer { get; set; }


        public static void createStream()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void ReceiveMessage()
        {
            while (true)
            {
                byte[] data = new byte[10000000];
                StringBuilder builder = new StringBuilder();
                int bytes;
                try
                {
                    do
                    {
                        try
                        {
                                bytes = stream.Read(data, 0, data.Length);
                            try
                            {
                                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                            }
                            catch
                            {
                                Image image = (Bitmap)((new ImageConverter()).ConvertFrom(data));
                                image.Save(@"D:\photo_test.jpg");
                            }
                        }
                        catch
                        {
                            Disconnect();
                            SystemErrorConnectToServer(false);
                            Thread.Sleep(1500);
                            continue;
                        }
                    }
                    while (stream.DataAvailable);
                }
                catch
                {
                    SystemErrorConnectToServer(true);
                }

                string textReceiveMessage = builder.ToString();

                 



                var jSendAfterLoginSchemaFrame = NJsonSchema.JsonSchema.FromType<JSendAfterLogin>();

                var userMessagesListSchemaFrame = NJsonSchema.JsonSchema.FromType<userMessagesList>();

                var messageEventSchemaFrame = NJsonSchema.JsonSchema.FromType<MessageEvent>();


                JSchema jSendAfterLoginSchema = JSchema.Parse(jSendAfterLoginSchemaFrame.ToJson().ToString());

                JSchema userMessagesListSchema = JSchema.Parse(userMessagesListSchemaFrame.ToJson().ToString());

                JSchema messageEventSchema = JSchema.Parse(messageEventSchemaFrame.ToJson().ToString());




                //var jSendAfterLoginSchemaFrame = NJsonSchema.JsonSchema.FromType<JSendAfterLogin>();
                ////var openCorrespondenceSchemaFrame = NJsonSchema.JsonSchema.FromType<OpenCorrespondence>();

                //JSchema jSendAfterLoginSchema = JSchema.Parse(jSendAfterLoginSchemaFrame.ToJson().ToString());

                //var jSendAfterLoginSchemaFrame = NJsonSchema.JsonSchema.FromType<JSendAfterLogin>();
                ////var openCorrespondenceSchemaFrame = NJsonSchema.JsonSchema.FromType<OpenCorrespondence>();

                //JSchema jSendAfterLoginSchema = JSchema.Parse(jSendAfterLoginSchemaFrame.ToJson().ToString());

                //= JsonConvert.DeserializeObject<JObject>(textReceiveMessage);

                    var message_Json = JObject.Parse(textReceiveMessage);

                if (message_Json.IsValid(jSendAfterLoginSchema))
                {
                    JSendAfterLogin jSend = JsonConvert.DeserializeObject<JSendAfterLogin>(textReceiveMessage);

                    receiveLoginEv(jSend);
                }

                else if (message_Json.IsValid(userMessagesListSchema))
                {

                    var jSend = JsonConvert.DeserializeObject<userMessagesList>(textReceiveMessage);
                    UserMessListItem(jSend, true);
                }

                else if (message_Json.IsValid(messageEventSchema))
                {
                    
                    var jSend = JsonConvert.DeserializeObject<MessageEvent>(textReceiveMessage);
                    if(jSend.statusType== "sendRespounse")
                    {
                        OpenCorrespondence openCorrespondence = new OpenCorrespondence("chat", System.Convert.ToInt32(ChatWindow.selectedId));
                        ConnectToServer.SendRequestOpenCorr(openCorrespondence);
                    }
                    UserMessEvent(jSend, true);
                }


                else
                {
                    SystemErrorConnectToServer(false);
                    Thread.Sleep(1500);
                    continue;
                }
            }
        }

        public static void SendRequest()
        {
            LoginEvent @event = new LoginEvent(loginToServer, passwordToServer);
            var LoginEvent = @event;
            string jSend = JsonConvert.SerializeObject(LoginEvent, Formatting.Indented);

            byte[] data = Encoding.Unicode.GetBytes(jSend.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void SendRequestMessEv(MessageEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            var message_Json = JObject.Parse(jSend);
             
            byte[] data = Encoding.Unicode.GetBytes(message_Json.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void SendRequestLogin(LoginEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            byte[] data = Encoding.Unicode.GetBytes(jSend.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void SendRequestOpenCorr(OpenCorrespondence @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            byte[] data = Encoding.Unicode.GetBytes(jSend.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }
}

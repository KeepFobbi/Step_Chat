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
using System.IO;

namespace Chat
{
    public static class ConnectToServer
    {
        //37.115.128.11  178.92.84.69
        //private const string host = "127.0.0.1";
        private const string host = "37.115.128.11";
        private const int port = 777;
        static TcpClient client;
        public static NetworkStream stream;

        public delegate void receiveLogin(JSendAfterLogin mess);
        public static event receiveLogin receiveLoginEv;
        public delegate void UserMess(MessageEvent mess);
        public static event UserMess UserMessEvent;
        public delegate void UserMessList(userMessagesList mess);
        public static event UserMessList UserMessListItem;

        public static string loginToServer { get; set; }
        public static string passwordToServer { get; set; }


        public static void createStream()
        {
            try
            {
                client = new TcpClient();
                client.Connect(host, port);
                stream = client.GetStream();

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch
            {
                Disconnect();
                Thread.Sleep(1000);
                createStream();
            }
        }

        public static void ReceiveMessage()
        {
            try
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    BinaryReader binaryReader = new BinaryReader(stream, Encoding.Unicode);

                    try
                    {
                        builder.Append(binaryReader.ReadString());
                    }
                    catch
                    {
                        //Image image = (Bitmap)((new ImageConverter()).ConvertFrom(data));
                        //image.Save(@"D:\photo_test.jpg");
                    }

                    string textReceiveMessage = builder.ToString();

                    var jSendAfterLoginSchemaFrame = NJsonSchema.JsonSchema.FromType<JSendAfterLogin>();
                    var userMessagesListSchemaFrame = NJsonSchema.JsonSchema.FromType<userMessagesList>();
                    var messageEventSchemaFrame = NJsonSchema.JsonSchema.FromType<MessageEvent>();

                    JSchema jSendAfterLoginSchema = JSchema.Parse(jSendAfterLoginSchemaFrame.ToJson().ToString());
                    JSchema userMessagesListSchema = JSchema.Parse(userMessagesListSchemaFrame.ToJson().ToString());
                    JSchema messageEventSchema = JSchema.Parse(messageEventSchemaFrame.ToJson().ToString());
                    try
                    {
                        var message_Json = JObject.Parse(textReceiveMessage);

                        if (message_Json.IsValid(jSendAfterLoginSchema))
                        {
                            JSendAfterLogin jSend = JsonConvert.DeserializeObject<JSendAfterLogin>(textReceiveMessage);
                            receiveLoginEv(jSend);
                        }
                        else if (message_Json.IsValid(userMessagesListSchema))
                        {
                            var jSend = JsonConvert.DeserializeObject<userMessagesList>(textReceiveMessage);
                            UserMessListItem(jSend);
                        }
                        else if (message_Json.IsValid(messageEventSchema))
                        {
                            var jSend = JsonConvert.DeserializeObject<MessageEvent>(textReceiveMessage);
                            UserMessEvent(jSend);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }

        public static void SendRequest()
        {
            LoginEvent @event = new LoginEvent(loginToServer, passwordToServer);
            var LoginEvent = @event;
            string jSend = JsonConvert.SerializeObject(LoginEvent, Formatting.Indented);

            BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.Unicode);
            binaryWriter.Write(jSend);
        }

        public static void SendRequestMessEv(MessageEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.Unicode);
            binaryWriter.Write(jSend);
        }

        public static void SendRequestLogin(LoginEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.Unicode);
            binaryWriter.Write(jSend);
        }

        public static void SendRequestOpenCorr(OpenCorrespondence @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.Unicode);
            binaryWriter.Write(jSend);
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Chat
{
    public static class ConnectToServer
    {
        //37.115.128.11  178.92.84.69
        private const string host = "178.92.84.69";
        //private const string host = "37.115.128.11";
        private const int port = 9090;
        private const int V = 16384;
        static TcpClient client;
        static NetworkStream stream;
        static Thread receiveThread;

        public delegate void receiveLogin(JSendAfterLogin mess);
        public static event receiveLogin receiveLoginEv;
        public delegate void UserMess(List<userMessages> mess, bool totalFlag);
        public static event UserMess UserMessEvent;
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

                receiveThread = new Thread(new ThreadStart(ReceiveMessage));
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
                byte[] data = new byte[550000];
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

                if (textReceiveMessage != "")
                {
                    if (textReceiveMessage.Contains(passwordToServer))
                    {
                        JSendAfterLogin jSend = JsonConvert.DeserializeObject<JSendAfterLogin>(textReceiveMessage);
                        receiveLoginEv(jSend);
                    }
                    else if (textReceiveMessage.Contains("group"))
                    {
                        textReceiveMessage = textReceiveMessage.Remove(0, 5);
                        var jSend = JsonConvert.DeserializeObject<List<userMessages>>(textReceiveMessage);
                        UserMessEvent(jSend, true);
                    }
                    else if (textReceiveMessage.Contains("chat"))
                    {
                        textReceiveMessage = textReceiveMessage.Remove(0,4);
                        var jSend = JsonConvert.DeserializeObject<List<userMessages>>(textReceiveMessage);
                        UserMessEvent(jSend, false);
                    }
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

        public static void SendRequest(MessageEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);
            

            byte[] data = Encoding.Unicode.GetBytes(jSend.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void SendRequest(LoginEvent @event)
        {
            var jSend = JsonConvert.SerializeObject(@event, Formatting.Indented);

            byte[] data = Encoding.Unicode.GetBytes(jSend.ToString());
            stream.Write(data, 0, data.Length);
        }

        public static void SendRequest(OpenCorrespondence @event)
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
            receiveThread.Abort();
        }
    }
}

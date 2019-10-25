using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания

        [Obsolete]
        static void Main(string[] args)
        {
            
                //TcpChannel serverChannel = new TcpChannel(8080);
                //ChannelServices.RegisterChannel(serverChannel);

                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            
        
        }
    }
}
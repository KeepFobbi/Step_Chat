using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Chat
{
    public partial class UserControlMessageReceived : UserControl
    {
        public int Id { get; set; }

        public UserControlMessageReceived()
        {
            InitializeComponent();
            ConnectToServer.UserMessEvent += MessEventCh;
        }

        public void MessEventCh(MessageEvent mess)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                switch (mess.statusType)
                {
                    case "DeleteRespounse":
                        if (mess.messages.Count == 1 && mess.messages.ContainsKey(Id))
                            this.Visibility = Visibility.Collapsed;
                        break;
                    case "UpdateRespounse":
                        if (mess.messages.ContainsKey(Id))
                            messageText.Text = mess.messages.Values.ElementAt(0);
                        break;
                    default:
                        break;
                }
            }));
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //TextBlock tbl = (TextBlock) SOMETHING???;
            //MessageBox.Show(tbl.Text);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //TextBlock tbl = (TextBlock) SOMETHING???;
            //MessageBox.Show(tbl.Text);
        }
    }
}

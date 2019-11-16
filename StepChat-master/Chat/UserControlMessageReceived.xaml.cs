using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chat
{
    public partial class UserControlMessageReceived : UserControl
    {
        public int Id { get; set; }

        public UserControlMessageReceived()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //TextBlock tbl = (TextBlock) SOMETHING???;
            //MessageBox.Show(tbl.Text);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageEvent @event = new MessageEvent("Delete", "chat", ChatWindow.selectedId, System.DateTime.Now, Id, "");
            ConnectToServer.SendRequest(@event);

            OpenCorrespondence openCorrespondence = new OpenCorrespondence("chat", System.Convert.ToInt32(ChatWindow.selectedId));
            ConnectToServer.SendRequest(openCorrespondence);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //TextBlock tbl = (TextBlock) SOMETHING???;
            //MessageBox.Show(tbl.Text);
        }
    }
}

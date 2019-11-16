using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chat
{
    /// <summary>
    /// Interaction logic for UserControlMessageSent.xaml
    /// </summary>
    public partial class UserControlMessageSent : UserControl
    {
        public int Id { get; set; }

        public UserControlMessageSent()
        {
            InitializeComponent();
        }

        public void changeText(string messageText)
        {
            this.messageText.Text = messageText;
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

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            TextBoxGrid.Visibility = Visibility.Visible;            
        }

        private void messageTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (messageTextBox.Text == "")
                    e.Handled = true;
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    this.messageTextBox.Text += "\n";
                    this.messageTextBox.CaretIndex = this.messageTextBox.Text.Length;
                    e.Handled = true;
                }
                else if (messageTextBox.Text != "")
                {
                    e.Handled = true;
                    messageText.Text = messageTextBox.Text;

                    TextBoxGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        //private void messageText_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show(Id.ToString());

        //    //MessageEvent @event = new MessageEvent("Delete", "chat", ChatWindow.selectedId, System.DateTime.Now, Id, "");
        //    //ConnectToServer.SendRequest(@event);
        //}
    }
}

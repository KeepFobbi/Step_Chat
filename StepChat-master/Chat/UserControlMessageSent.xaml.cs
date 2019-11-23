using Newtonsoft.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chat
{
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

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageEvent @event = new MessageEvent("Delete", "chat", ChatWindow.selectedId, ChatWindow.userId, System.DateTime.Now, Id, "");
        
            ConnectToServer.SendRequestMessEv(@event);

            Visibility = Visibility.Collapsed;
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            TextBoxGrid.Visibility = Visibility.Visible;
            messageTextBox.Focus();
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

                    MessageEvent @event = new MessageEvent("UpdateRespounse", "chat", ChatWindow.selectedId,
                        ChatWindow.userId, System.DateTime.Now, Id, messageText.Text);

                    ConnectToServer.SendRequestMessEv(@event);
                }
            }
        }
    }
}

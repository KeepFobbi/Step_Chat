using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Input;
using System.IO;

namespace Chat
{
    public partial class Login : Window
    {
        string path = @"states.dat";

        public Login()
        {
            InitializeComponent();

            loginBox.Text = "Fobbi";
            passwordBox.Password = "Fobbi";
            //Button_Click(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)RememberMe.IsChecked)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    writer.Write(loginBox.Text);
                    writer.Write(passwordBox.Password);
                }
            }
            Enter(loginBox.Text, passwordBox.Password.ToString());
        }

        private bool Enter(string login, string pass)
        {
            try
            {
                ConnectStatus.Visibility = Visibility.Visible;
                Thread receiveThread = new Thread(new ThreadStart(ConnectToServer.createStream));
                receiveThread.Start();
                ConnectToServer.receiveLoginEv += createMainWindow;
                ConnectToServer.loginToServer = login;
                ConnectToServer.passwordToServer = pass;
                ConnectToServer.SendRequest();

                return true;
            }
            catch
            {
                return false;
            }
        }

        ChatWindow chatWindow = new ChatWindow();
        private void createMainWindow(JSendAfterLogin jSend)
        {
            if (jSend.User.userPassword == passwordBox.Password.ToString())
            {
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    chatWindow.Show();
                    Close();
                }));
            }
            else
            {
                loginBox.Foreground = Brushes.White;
                loginBox.Background = Brushes.OrangeRed;
                passwordBox.Foreground = Brushes.White;
                passwordBox.Background = Brushes.OrangeRed;
                loginBox.Text = "Ошибка!";
                passwordBox.Clear();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ConnectToServer.Disconnect();
            Application.Current.Shutdown();
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loginBox.Foreground = Brushes.Gray;
            loginBox.Background = Brushes.White;
            passwordBox.Foreground = Brushes.Gray;
            passwordBox.Background = Brushes.White;
            loginBox.Clear();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        loginBox.Text = reader.ReadString();
                        passwordBox.Password = reader.ReadString();

                        Enter(loginBox.Text, passwordBox.Password.ToString());
                    }
                }
            }
        }
    }
}
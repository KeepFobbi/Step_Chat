﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

namespace Chat
{
    public partial class ChatWindow : Window
    {
        public static string selectedId = null;
        bool selectedStatus;
        List<ListViewItem> listViewI = new List<ListViewItem>();
        public static int userId;
        string MyUserName;
        string userPassword;
        string userFio;
        string userBirthday;
        string userPhone;

        public ChatWindow()
        {
            InitializeComponent();
            ConnectToServer.UserMessListItem += senderMessage;
            ConnectToServer.receiveLoginEv += addUserChat;
            messageTextBox.AcceptsReturn = true;
            messageTextBox.Focus();
        }

        public void ChangeMessTextBox(string messageTextBox)
        {
            this.messageTextBox.Focus();
            this.messageTextBox.Text += messageTextBox;
            Binding binding = new Binding();

            binding.ElementName = "messageText"; // элемент-источник
            binding.Path = new PropertyPath("Text"); // свойство элемента-источника
            this.messageTextBox.SetBinding(TextBlock.TextProperty, binding); // установка привязки для элемента-приемника
        }

        #region full
        public void addUserChat(JSendAfterLogin jSend)
        {
            userId = jSend.User.userId; ;
            MyUserName = jSend.User.userName;
            userPassword = jSend.User.userPassword;
            userFio = jSend.User.userFio;
            userBirthday = jSend.User.userBirthday.ToShortDateString().ToString();
            userPhone = jSend.User.userPhone;
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                for (int i = 0; i < jSend.startInfos.Count; i++)
                {
                    TextBlock textBlock;
                    listViewItem_1.Visibility = Visibility.Visible;
                    string lvi = XamlWriter.Save(listViewItem_1);
                    StringReader stringReader = new StringReader(lvi);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    ListViewItem listViewItem = (ListViewItem)XamlReader.Load(xmlReader);
                    listViewI.Add(listViewItem);

                    textBlock = (TextBlock)listViewItem.FindName("userName");
                    if (jSend.startInfos[i].groupId != null)
                    {

                        textBlock.Text = jSend.startInfos[i].groupName;
                        listViewItem.Name = "id" + jSend.startInfos[i].groupId.ToString();
                        listViewItem.Tag = true;
                    }
                    else
                    {
                        textBlock.Text = jSend.startInfos[i].userName;
                        listViewItem.Name = "id" + jSend.startInfos[i].chatId.ToString();
                        listViewItem.Tag = false;
                    }
                    if (jSend.startInfos[i].content != null)
                    {
                        textBlock = (TextBlock)listViewItem.FindName("lastMessage");
                        textBlock.Text = jSend.startInfos[i].content;
                    }
                    else
                        lastMessage.Text = "";


                    listViewItem.Selected += ListViewItem_1_Selected;


                    listView1.Items.Add(listViewItem);
                }
                listView1.Items[0] = null;
                scrollViewerListItem.ScrollToBottom();
            }));
        }

        private void SentButton_Click(object sender, RoutedEventArgs e)
        {
            UserControlMessageSent userControlMessageSent = new UserControlMessageSent();
            userControlMessageSent.changeText(messageTextBox.Text);
            userControlMessageSent.HorizontalAlignment = HorizontalAlignment.Right;
            if (selectedId != null && !selectedStatus)
            {
                MessageEvent @event = new MessageEvent("Send", "chat", selectedId, userId, DateTime.Now, -1, messageTextBox.Text);
                ConnectToServer.SendRequestMessEv(@event);
            }
            //ConnectToServer.SendRequest($"send chat {selectedId} {DateTime.Now.ToShortTimeString()} {messageTextBox.Text}");
            else
            {
                MessageEvent @event = new MessageEvent("Send", "group", selectedId, userId, DateTime.Now, -1, messageTextBox.Text);
                ConnectToServer.SendRequestMessEv(@event);
            }
            //ConnectToServer.SendRequest($"send group {selectedId} {DateTime.Now.ToShortTimeString()} {messageTextBox.Text}");
            userControlMessageSent.timeSent.Text = DateTime.Now.ToShortTimeString().ToString();
            messagePlace.Children.Add(userControlMessageSent);
            messageTextBox.Clear();
            scrollViewerMess.ScrollToBottom();
        }

        public void senderMessage(userMessagesList mess)
        {
            if (mess.uMList.Count > 1)
                for (int i = 0; i < mess.uMList.Count; i++)
                {
                    if (mess.uMList[i].senderId == userId)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            UserControlMessageSent userControlMessageSent = new UserControlMessageSent();
                            userControlMessageSent.changeText(mess.uMList[i].content);
                            userControlMessageSent.Id = mess.uMList[i].messageId;
                            userControlMessageSent.HorizontalAlignment = HorizontalAlignment.Right;
                            userControlMessageSent.timeSent.Text = mess.uMList[i].createAt.ToString();
                            messagePlace.Children.Add(userControlMessageSent);
                            scrollViewerMess.ScrollToBottom();
                            messageTextBox.Focus();
                        }));
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            UserControlMessageReceived userControlMessageReceived = new UserControlMessageReceived();
                            userControlMessageReceived.timeRec.Text = mess.uMList[i].createAt.ToString();
                            userControlMessageReceived.messageText.Text = mess.uMList[i].content;
                            userControlMessageReceived.Id = mess.uMList[i].messageId;
                            userControlMessageReceived.HorizontalAlignment = HorizontalAlignment.Left;
                            messagePlace.Children.Add(userControlMessageReceived);
                            scrollViewerMess.ScrollToBottom();
                            messageTextBox.Focus();
                        }));
                    }
                }
            else
            {
                if (mess.uMList[0].recipientGroupId == Convert.ToInt32(selectedId))
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        UserControlMessageReceived userControlMessageReceived = new UserControlMessageReceived();
                        userControlMessageReceived.timeRec.Text = mess.uMList[0].createAt.ToString();
                        userControlMessageReceived.messageText.Text = mess.uMList[0].content;
                        userControlMessageReceived.Id = mess.uMList[0].messageId;
                        userControlMessageReceived.HorizontalAlignment = HorizontalAlignment.Left;
                        messagePlace.Children.Add(userControlMessageReceived);
                        scrollViewerMess.ScrollToBottom();
                        messageTextBox.Focus();
                    }));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        foreach (ListViewItem lvi in this.listViewI)
                        {
                            if (lvi.Name == $"id{mess.uMList[0].recipientGroupId}")
                            {
                                ListViewItem listViewItem = (ListViewItem)listViewI[listViewI.IndexOf(lvi)];
                                if ((bool)listViewItem.Tag)
                                {
                                    Border border = (Border)listViewItem.FindName("messageBorderCount");
                                    TextBlock block = (TextBlock)listViewItem.FindName("messageCount");
                                    border.Visibility = Visibility.Visible;
                                    block.Text = Convert.ToString(Convert.ToInt32(block.Text) + 1);
                                    messageTextBox.Focus();
                                }

                            }
                        }
                    }));
                }

                if (mess.uMList[0].recipientChatId == Convert.ToInt32(selectedId))
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        UserControlMessageReceived userControlMessageReceived = new UserControlMessageReceived();
                        userControlMessageReceived.timeRec.Text = mess.uMList[0].createAt.ToString();
                        userControlMessageReceived.messageText.Text = mess.uMList[0].content;
                        userControlMessageReceived.Id = mess.uMList[0].messageId;
                        userControlMessageReceived.HorizontalAlignment = HorizontalAlignment.Left;
                        messagePlace.Children.Add(userControlMessageReceived);
                        messageTextBox.Focus();
                        scrollViewerMess.ScrollToBottom();
                    }));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        foreach (ListViewItem lvi in this.listViewI)
                        {
                            if (lvi.Name == $"id{mess.uMList[0].recipientChatId}")
                            {
                                ListViewItem listViewItem = (ListViewItem)listViewI[listViewI.IndexOf(lvi)];
                                if (!(bool)listViewItem.Tag)
                                {
                                    Border border = (Border)listViewItem.FindName("messageBorderCount");
                                    TextBlock block = (TextBlock)listViewItem.FindName("messageCount");
                                    border.Visibility = Visibility.Visible;
                                    block.Text = Convert.ToString(Convert.ToInt32(block.Text) + 1);
                                    messageTextBox.Focus();
                                }
                            }
                        }
                    }));
                }
            }
        }



        private void ListViewItem_1_Selected(object sender, RoutedEventArgs e)
        {
            this.Width = 1080;
            InfoPanel.Visibility = Visibility.Visible;
            MessPanelGrid.Visibility = Visibility.Visible;
            MessPanel.Visibility = Visibility.Visible;
            messagePlace.Children.Clear();

            object obj = e.Source;
            ListViewItem viewItem = (ListViewItem)obj;
            TextBlock block = (TextBlock)viewItem.FindName("userName");
            Border border = (Border)viewItem.FindName("messageBorderCount");
            border.Visibility = Visibility.Collapsed;
            selectedId = viewItem.Name.Replace("id", "");
            chatName.Text = chatTitle.Text = block.Text;
            selectedStatus = (bool)viewItem.Tag;

            if ((bool)viewItem.Tag)
            {
                OpenCorrespondence openCorrespondence = new OpenCorrespondence("group", Convert.ToInt32(selectedId));
                ConnectToServer.SendRequestOpenCorr(openCorrespondence);
            }
            //ConnectToServer.SendRequest($"group {selectedId}");
            else
            {
                OpenCorrespondence openCorrespondence = new OpenCorrespondence("chat", Convert.ToInt32(selectedId));
                ConnectToServer.SendRequestOpenCorr(openCorrespondence);
            }
            //ConnectToServer.SendRequest($"chat {selectedId}");

            MessPanel.Visibility = Visibility.Visible;
        }

        private void MessageTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
                    SentButton_Click(null, null);
                }
            }
        }

        bool stateFlag = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stateFlag == false)
            {
                this.MaxWidth = SystemParameters.WorkArea.Width;
                this.MaxHeight = SystemParameters.WorkArea.Height;
                WindowState = WindowState.Maximized;
                winStateImg.Kind = MaterialDesignThemes.Wpf.PackIconKind.FullscreenExit;
                stateFlag = true;
            }
            else
            {
                winStateImg.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
                WindowState = WindowState.Normal;
                stateFlag = false;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ConnectToServer.Disconnect();
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int A = 440, D = 587, H = 493, C = 523;
            Console.Beep(D, 150);
            Console.Beep(D, 150);
            Console.Beep(H, 150);
            Console.Beep(C, 500);
            Console.Beep(A, 450);
            Console.Beep(A, 300);
        }
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock block;
            foreach (ListViewItem item in listViewI)
            {
                block = (TextBlock)item.FindName("userName");
                if (block.Text.Contains(searchBox.Text))
                {
                    listView1.Items.Clear();
                    listView1.Items.Add(item);
                }
                if (searchBox.Text == "")
                {
                    e.Handled = true;
                    listView1.Items.Clear();
                    for (int i = 0; i < listViewI.Count; i++)
                        listView1.Items.Add(listViewI[i]);
                    return;
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyUserInfo info = new MyUserInfo();
            info.chatName.Text = userFio;
            info.descriptionInfo.Visibility = Visibility.Collapsed;
            info.aboutMe.Visibility = Visibility.Collapsed;
            info.Username.Text = MyUserName;
            info.Phone.Text = userPhone;
            info.Birthday.Text = userBirthday;
            info.Show();
        }

        private void Border_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            messageTextBox.Focus();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (PlusButton.Kind != MaterialDesignThemes.Wpf.PackIconKind.MinusCircle)
            {
                PlusButton.Kind = MaterialDesignThemes.Wpf.PackIconKind.MinusCircle;
                this.Width = 1080;
                InfoPanel.Visibility = Visibility.Visible;
                MessPanelGrid.Visibility = Visibility.Visible;
                MessPanel.Visibility = Visibility.Visible;
            }
            else
            {
                PlusButton.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlusCircle;
                Width = 275;
                InfoPanel.Visibility = Visibility.Hidden;
                MessPanelGrid.Visibility = Visibility.Hidden;
                MessPanel.Visibility = Visibility.Hidden;
            }
        }
    }
}
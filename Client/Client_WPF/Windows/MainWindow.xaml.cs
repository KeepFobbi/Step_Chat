using Client_WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        

        public MainWindow()
        {
            InitializeComponent();

            Login l = new Login();
            l.ShowDialog();

            
                 
                 




        }

       /* static void SendMessage()
        {
           

            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }*/

        



        /* public enum pages
         {
             login,
             registration,
             working
         }

       public void OpenPage(pages page)
         {
             if(page==pages.login)
             {
                 Frame_main.Navigate(new Login(this));
             }
            else if (page==pages.working)
             {
                 Frame_main.Navigate(new Working(this));
             }

         }*/


    }
}

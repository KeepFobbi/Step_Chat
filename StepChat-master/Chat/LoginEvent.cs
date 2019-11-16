using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class LoginEvent
    {
        public string login { get; set; }
        public string password { get; set; }

        public LoginEvent(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}

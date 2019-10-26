using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string userFio { get; set; }
        public DateTime userBirthday { get; set; }
        public string userPhone { get; set; }
        //public byte[] userImage { get; set; }

    }
}

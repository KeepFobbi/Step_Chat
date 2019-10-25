using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.BD_Entities
{
    class User
    {
        [Key]
        public int userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string userFio { get; set; }
        public DateTime userBirthday { get; set; }
        public string userPhone { get; set; }
        public byte[] userImage { get; set; }
        

    }
}
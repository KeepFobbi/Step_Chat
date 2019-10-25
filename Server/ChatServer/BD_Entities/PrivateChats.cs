using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.BD_Entities
{
    class PrivateChats
    {
        [Key]
        public int chatId { get; set; }
        public int user_1_Id { get; set; }
        public int user_2_Id { get; set; }

    }
}


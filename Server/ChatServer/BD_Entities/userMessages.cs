using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.BD_Entities
{
    class userMessages
    {
        [Key]
        public int messageId { get; set; }
        public int senderId { get; set; }
        public Nullable<int> recipientGroupId { get; set; }
        public Nullable<int> recipientChatId { get; set; }
        public DateTime createAt { get; set; }
        public Nullable <DateTime> updateAt { get; set; }
        public string content { get; set; }

        


            

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepServer
{
    class userMessages
    {
        public int MessageId { get; set; }
        public int senderId { get; set; }
        public int recipientId { get; set; }
        public int contentId { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}

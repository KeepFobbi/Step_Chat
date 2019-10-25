using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class StartInfo
    {
        public Nullable<int> groupId { get; set; }
        public Nullable<int> chatId { get; set; }
        public string groupName { get; set; }
        public string userName { get; set; }
        public string content { get; set; }
    }
}

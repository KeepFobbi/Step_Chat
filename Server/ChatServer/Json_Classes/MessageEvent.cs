using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Json_Classes
{
    class MessageEvent
    {
        

            public string statusType;
            public string recipientTtype;
            public int recipientIid;
            public DateTime eventTime;
            public Dictionary<int, string> messages;
        
    }
}

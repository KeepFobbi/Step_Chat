using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Json_Classes
{
    public class MessageEvent
    {

        public string statusType;
        public string recipientTtype;
        public int recipientIid;
        public DateTime eventTime;
        public Dictionary<int, string> messages = new Dictionary<int, string>();

        public MessageEvent(string statusType, string recipientTtype, string recipientIid, DateTime eventTime, int tempIdMess, string content)
        {
            this.statusType = statusType;
            this.recipientTtype = recipientTtype;
            this.recipientIid = Convert.ToInt32(recipientIid);
            this.eventTime = eventTime;
            this.messages.Add(tempIdMess, content);
        }

    }
}

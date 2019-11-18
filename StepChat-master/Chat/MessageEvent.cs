using System;
using System.Collections.Generic;

namespace Chat
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

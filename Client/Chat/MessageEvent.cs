using System;
using System.Collections.Generic;

namespace Chat
{
    public class MessageEvent
    {
        public string statusType;
        public string recipientTtype; // chat or group в сторону кого идет..
        public int recipientIid; // id получателя
        public DateTime eventTime; // время изменения
        public Dictionary<int, string> messages = new Dictionary<int, string>(); // int - id сообщения, string - контент

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

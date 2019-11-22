using System;
using System.Collections.Generic;

namespace Chat
{
    public class MessageEvent
    {
        public string statusType;
        public string recipientTtype;  // чат или группа
        public int recipientId; // Получатель
        public int senderId; // Отправитель
        public DateTime eventTime;
        public Dictionary<int, string> messages = new Dictionary<int, string>();

        public MessageEvent(string statusType, string recipientTtype, string recipientIid, int senderId, DateTime eventTime, int tempIdMess, string content)
        {
            this.statusType = statusType;
            this.recipientTtype = recipientTtype;
            this.recipientId = Convert.ToInt32(recipientIid);
            this.senderId = senderId;
            this.eventTime = eventTime;
            this.messages.Add(tempIdMess, content);
        }
    }
}

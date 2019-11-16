using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class Groups
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public byte[] groupImage { get; set; }
        public int ownerid { get; set; }
    }
}

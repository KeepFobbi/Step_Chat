using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.BD_Entities
{
    class Groups
    {
        [Key]
        public int groupId { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public byte[] groupImage { get; set; }
        public int ownerid { get; set; }
    }
}

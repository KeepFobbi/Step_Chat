using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.BD_Entities
{
    class GroupsMembers
    {
        [Key]
        [Column(Order = 1)]
        public int userId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int groupId { get; set; }
    }
}

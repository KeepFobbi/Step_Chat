using ChatServer.BD_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class UserContext : DbContext
    {
        public UserContext() : base("DBConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<GroupsMembers> GroupsMembers { get; set; }
        public DbSet<userMessages> userMessages { get; set; }
        public DbSet<PrivateChats> PrivateChats { get; set; }

    }
}

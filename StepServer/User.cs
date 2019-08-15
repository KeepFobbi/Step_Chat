using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepServer
{
    class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string passwordUser { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public DateTime Birthday { get; set; }
        public string phoneNum { get; set; }
        public string groupName { get; set; }
    }
}
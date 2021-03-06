using System.Collections;
using System.Collections.Generic;

namespace Broker_Shuranskiy.Models
{  
    public class Users
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public float Balance { get; set; } = 50000;
        public string Role { get; set; } = "guest";
    }
}

using System.Collections;
using System.Collections.Generic;

namespace Broker_Shuranskiy.Models
{  
    public enum _Role {admin, user} 
    public class Users
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public float Balance { get; set; } = 50000;
        public HashSet<Bags> Bag { get; set; }
      
        public _Role Role { get; set; }
    }
}

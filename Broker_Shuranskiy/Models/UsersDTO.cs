namespace Broker_Shuranskiy.Models
{
    public class UsersDTO
    {
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }


        public static explicit operator Users (UsersDTO ud)
        {
            Users users = new Users
            {
                First_Name = (string)ud.First_Name,
                Second_Name = (string)ud.Second_Name,
                User_Name = (string)ud.User_Name,
                Password = (string)ud.Password
            };
            return users;
        }
    }
}

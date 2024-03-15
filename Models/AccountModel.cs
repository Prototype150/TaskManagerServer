using System.ComponentModel;

namespace Models
{
    public class AccountModel
    {
        public AccountModel(string username, string password) { 
            Username = username;
            Password = password;
            Id = -1;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

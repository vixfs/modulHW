using System;

namespace MoneyService.Models.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        
        /*public override string ToString()
        {
            return $"Id:{Id} Email:{Email} Username:{Username} PasswordHash:{PasswordHash} PasswordSalt:{PasswordSalt}";
        }*/
    }
}
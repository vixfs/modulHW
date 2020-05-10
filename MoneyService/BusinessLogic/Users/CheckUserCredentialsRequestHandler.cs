using System;
using System.Security.Cryptography;
using System.Text;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;

namespace MoneyService.BusinessLogic.Users
{
    public class CheckUserCredentialsRequestHandler
    {
        private readonly IUserService _userService;

        public CheckUserCredentialsRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(string.Concat(password, storedSalt));
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return storedHash == sb.ToString();
            }
        }

        public User Handle(UserCredentials userCredentials)
        {
            var user = _userService.GetByEmail(userCredentials.Email).Result;

            if (user == null)
            {
                throw new ArgumentException("Пользователя с таким email не существует", userCredentials.Email);
            }
            
            if (!VerifyPasswordHash(userCredentials.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ArgumentException("Введен неверный пароль");
            }

            return user;
        }
        
    }
}
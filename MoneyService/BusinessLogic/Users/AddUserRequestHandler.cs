using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;

namespace MoneyService.BusinessLogic.Users
{
    public class AddUserRequestHandler
    {
        private readonly IUserService _userService;

        public AddUserRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            passwordSalt = Guid.NewGuid().ToString().Substring(0, 8);
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(string.Concat(password, passwordSalt));
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                var passwordHash1 = sb.ToString();
                passwordHash = passwordHash1;
            }
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        public Task<User> Handle(RegisterModel registerModel)
        {
            if (!IsValidEmail(registerModel.Email))
            {
                throw new ArgumentException("Неверный формат email", registerModel.Email);
            }
            if (_userService.GetByEmail(registerModel.Email).Result != null)
            {
                throw new ArgumentException("Пользователь с таким email уже зарегистрирован", registerModel.Email);
            }
            if (string.IsNullOrWhiteSpace(registerModel.Password))
            {
                throw new ArgumentException("Пароль не введен", registerModel.Password);
            }
            if (registerModel.Password.Length < 8)
            {
                throw new ArgumentException("Слишком короткий пароль", registerModel.Password);
            }
            
            string passwordHash, passwordSalt;
            CreatePasswordHash(registerModel.Password, out passwordHash, out passwordSalt);
            
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerModel.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            
            return _userService.AppendUser(user);
        }
    }
}
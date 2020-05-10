using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MoneyService.Models.Accounts;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;
using Npgsql;

namespace MoneyService.Services
{
    public class UserService : IUserService
    {
        private const string ConnectionString = "host=localhost;port=5432;database=moneyservice;username=postgres;password=1";
        
        public async Task<User> GetByEmail(string email)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE email = @email",
                    new {email});
            }
        }
        
        public async Task<User> GetById(string userId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @userId",
                    new {userId});
            }
        }
        
        
        public async Task<User> AppendUser(User user)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleAsync<User>("INSERT INTO Users (Id, Email, PasswordHash, PasswordSalt) VALUES (@Id, @Email, @PasswordHash, @PasswordSalt) RETURNING *", user);
            }
        }
        
        /*public async Task<User> IsValidUser(string currentEmail, string password)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                
                var user = connection.QuerySingleOrDefault<User>("SELECT Id, Email, PasswordSalt, PasswordHash FROM Users WHERE Email = @currentEmail;",
                    new
                    {
                        currentEmail,
                    });
                
            }
        }*/
        
    }
    
}
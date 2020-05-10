using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.Models.Accounts;
using MoneyService.Models.Users;

namespace MoneyService.Services.Interfaces
{
    public interface IUserService
    {
        /*bool IsValidUser(string username, string password);*/
        Task<User> GetByEmail(string email);
        Task<User> GetById(string userId);
        Task<User> AppendUser(User user);
        
        /*User Create(User user, string password);*/
    }
}
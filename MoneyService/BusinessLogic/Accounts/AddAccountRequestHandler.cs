using System;
using System.Security.AccessControl;
using System.Threading.Tasks;
using MoneyService.Models.Accounts;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;

namespace MoneyService.BusinessLogic.Accounts
{
    public class AddAccountRequestHandler
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public AddAccountRequestHandler(
            IUserService userService, 
            IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }

        public long GenerateAccountNumber()
        {
            long accountNumber = 0;
            Random rnd = new Random();
            bool isNumberUnique = false;
            while (!isNumberUnique)
            {
                isNumberUnique = IsAccountIdUnique(accountNumber);
                accountNumber = rnd.Next(0, (int)(Math.Pow(10, 9))); // генерация 9-значного числа
                accountNumber = long.Parse("4" + accountNumber.ToString());
            }
            return accountNumber;
        }
        
        public bool IsAccountIdUnique(long accountNumber)
        {
            var account = _accountService.GetAccountByNumber(accountNumber).Result;
            if (account != null)
            {
                return false;
            }
            return true;
        }
        
        public Task<AccountModel> Handle(string userid)
        {
            if(_accountService.GetAccountByHolder(userid).Result != null)
            {
                throw new ArgumentException("Нельзя иметь больше одного счета"); 
            }
            var account = new AccountModel()
            {
                accountNumber = GenerateAccountNumber(),
                HolderID = userid
            };
            return _accountService.AppendAccount(account);
        }
    }
}

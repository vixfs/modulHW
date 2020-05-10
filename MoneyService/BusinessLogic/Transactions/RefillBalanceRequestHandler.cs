using System;
using System.Threading.Tasks;
using MoneyService.Controllers;
using MoneyService.Models.Accounts;
using MoneyService.Models.Transactions;
using MoneyService.Services.Interfaces;

namespace MoneyService.BusinessLogic.Transactions
{
    public class RefillBalanceRequestHandler
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IAccountService _accountService;

        public RefillBalanceRequestHandler(
            ITransactionsService transactionsService,
            IAccountService accountService
            )
        {
            _transactionsService = transactionsService;
            _accountService = accountService;
        }
        
        public Task<AccountModel> Handle(RefillModel refillModel, string currentUserId)
        {
            var account = _accountService.GetAccountByHolder(currentUserId).Result;
            if (account == null)
            {
                throw new ArgumentException("Ваш счет не открыт, пополнение невозможно");
            }
            
            refillModel.accountNumber = account.accountNumber;
            account.Balance += refillModel.amount;
            
            return _transactionsService.UpdateAccountBalance(account);
        }
        
    }
}
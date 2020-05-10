using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.BusinessLogic.Accounts;
using MoneyService.Models.Accounts;
using MoneyService.Models.Transactions;
using MoneyService.Services.Interfaces;

namespace MoneyService.BusinessLogic.Transactions
{
    public class TransferMoneyRequestHandler
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IAccountService _accountService;

        public TransferMoneyRequestHandler(
            ITransactionsService transactionsService,
            IAccountService accountService
        )
        {
            _transactionsService = transactionsService;
            _accountService = accountService;
        }
        
        public List<Task<AccountModel>> Handle(TransferModel transferModel, string currentUserId)
        {
            var senderAccount = _accountService.GetAccountByHolder(currentUserId).Result;
            if (senderAccount == null)
            {
                throw new ArgumentException("Ваш счет не открыт, пополнение невозможно");
            }

            transferModel.SenderAccountId = senderAccount.accountNumber;
            
            var recipientAccount = _accountService.GetAccountByNumber(transferModel.RecipientAccountId).Result;
            if (recipientAccount == null)
            {
                throw new ArgumentException("Введенного счета не существует");
            }

            if (senderAccount.Balance < transferModel.Amount)
            {
                throw new ArgumentException("Недостаточно средств для перевода");
            }

            senderAccount.Balance -= transferModel.Amount;
            recipientAccount.Balance += transferModel.Amount;

            var accountList = new List<Task<AccountModel>>();
            accountList.Add(_transactionsService.UpdateAccountBalance(senderAccount));
            accountList.Add(_transactionsService.UpdateAccountBalance(recipientAccount));

            return accountList;

            //return _transactionsService.UpdateSomeAccountsBalance(senderAccount, recipientAccount);
            //return _transactionsService.UpdateAccountBalance(senderAccount);

        }
    }
}
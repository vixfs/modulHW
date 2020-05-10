using System;

namespace MoneyService.Models.Accounts
{
    public class AccountModel
    {
        public long accountNumber { get; set; }
        public string HolderID { get; set; }
        public decimal Balance { get; set; }
    }
}
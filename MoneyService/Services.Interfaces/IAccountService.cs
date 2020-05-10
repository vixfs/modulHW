using System.Threading.Tasks;
using MoneyService.Models.Accounts;
using MoneyService.Models.Users;

namespace MoneyService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountModel> AppendAccount(AccountModel accountModel);
        Task<AccountModel> GetAccountByNumber(long accountNumber);
        Task<AccountModel> GetAccountByHolder(string accountHolderId);
    }
}
using System.Threading.Tasks;
using MoneyService.Models.Accounts;
using MoneyService.Models.Transactions;

namespace MoneyService.Services.Interfaces
{
    public interface ITransactionsService
    {
        Task<AccountModel> UpdateAccountBalance(AccountModel accountModel);
    }
}
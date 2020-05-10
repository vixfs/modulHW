using System.Threading.Tasks;
using Dapper;
using MoneyService.Models.Accounts;
using MoneyService.Models.Transactions;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;
using Npgsql;

namespace MoneyService.Services
{
    public class TransactionsService : ITransactionsService
    {
        private const string ConnectionString = "host=localhost;port=5432;database=moneyservice;username=postgres;password=1";
        
        public async Task<AccountModel> UpdateAccountBalance(AccountModel accountModel)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleAsync<AccountModel>("UPDATE accounts SET balance = @balance WHERE accountnumber = @accountNumber", new
                {
                    accountNumber = accountModel.accountNumber,
                    balance = accountModel.Balance
                });
                
            }
        }
    }
}
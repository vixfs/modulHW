using System.Threading.Tasks;
using Dapper;
using MoneyService.Models.Accounts;
using MoneyService.Services.Interfaces;
using Npgsql;

namespace MoneyService.Services
{
    public class AccountService : IAccountService
    {
        private const string ConnectionString = "host=localhost;port=5432;database=moneyservice;username=postgres;password=1";
        
        public async Task<AccountModel> AppendAccount(AccountModel accountModel)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleAsync<AccountModel>("INSERT INTO accounts (accountNumber, HolderId, Balance) VALUES (@accountNumber, @Holderid, @Balance) RETURNING *", accountModel);
            }
        }
        

        public async Task<AccountModel> GetAccountByNumber(long accountNumber)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<AccountModel>("SELECT * FROM accounts WHERE accountnumber = @accountNumber",
                    new {accountNumber});
            } 
        }
        
        public async Task<AccountModel> GetAccountByHolder(string accountHolderId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<AccountModel>("SELECT * FROM accounts WHERE holderid = @accountHolderId",
                    new {accountHolderId});
            } 
        }
        
        
        
        
        
    }
}
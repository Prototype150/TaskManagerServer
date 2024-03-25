using DAL.Interfaces;
using Models;
using Microsoft.EntityFrameworkCore;

namespace SQLDataManager
{
    public class AccountSQLDataManager : IAccountDataManager
    {
        private string _connectionString;

        public AccountSQLDataManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<AccountModel?> GetAccountByUsername(string username)

        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                var account = await db.Accounts.FirstOrDefaultAsync(x => x.Username == username);
                return account;
            }
        }

        public async Task<bool> IsExist(string username)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                return await db.Accounts.AnyAsync(x => x.Username == username);
            }
        }

        public async Task<bool> PostAccount(AccountModel newAccount)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                newAccount.Id = 0;
                var result = (await db.Accounts.AddAsync(newAccount)).Entity;
                await db.SaveChangesAsync();
                return result != null;
            }
        }
    }
}

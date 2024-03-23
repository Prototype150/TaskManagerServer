using DAL.Interfaces;
using Models;
using Microsoft.EntityFrameworkCore;

namespace SQLDataManager
{
    public class AccountSQLDataManager : IAccountDataManager, IDisposable
    {
        private string _connectionString;
        private TaskManagerDbContext _dbContext;

        public AccountSQLDataManager(string connectionString)
        {
            _connectionString = connectionString;
            _dbContext = new TaskManagerDbContext(connectionString);
        }

        public AccountModel? GetAccountByUsername(string username)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Username == username);
            return account;
        }

        public bool IsExist(string username)
        {
            _dbContext.Database.EnsureCreated();
            return _dbContext.Accounts.Any(x => x.Username == username);
        }

        public void PostAccount(AccountModel newAccount)
        {
            newAccount.Id = 0;
            _dbContext.Accounts.Add(newAccount);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

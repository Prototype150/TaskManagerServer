using TaskManagerServer.DAL.Interfaces;
using TaskManagerServer.Models;

namespace TaskManagerServer.DAL
{
    public class AccountMockDataManager : IAccountDataManager
    {
        private int counter;
        private List<AccountModel> _accounts;

        public AccountMockDataManager() {
            _accounts = new List<AccountModel>();
            counter = 0;
        }

        public AccountModel? GetAccountByUsername(string username)
        {
            return _accounts.FirstOrDefault(x => x.Username == username);
        }

        public bool IsExist(string username)
        {
            return _accounts.Any(x=> x.Username == username);
        }

        public void PostAccount(AccountModel newAccount)
        {
            newAccount.Id = counter++;
            _accounts.Add(newAccount);
        }
    }
}
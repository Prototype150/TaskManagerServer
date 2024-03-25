using DAL.Interfaces;
using Models;

namespace DAL
{
    public class AccountMockDataManager : IAccountDataManager
    {
        private int counter;
        private List<AccountModel> _accounts;

        public AccountMockDataManager() {
            _accounts = new List<AccountModel>();
            counter = 0;
        }

        public async Task<AccountModel?> GetAccountByUsername(string username)
        {
            return _accounts.FirstOrDefault(x => x.Username == username);
        }

        public async Task<bool> IsExist(string username)
        {
            return _accounts.Any(x=> x.Username == username);
        }

        public async Task<bool> PostAccount(AccountModel newAccount)
        {
            newAccount.Id = counter++;
            _accounts.Add(newAccount);
            return _accounts.Contains(newAccount);
        }
    }
}
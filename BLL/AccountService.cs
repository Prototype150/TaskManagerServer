

using BLL.Interfaces;
using DAL.Interfaces;
using Models;

namespace BLL
{
    public class AccountService : IAccountService
    {
        private IAccountDataManager _accountDataManager;
        public AccountService(IAccountDataManager accountDataManager) {
            _accountDataManager = accountDataManager;
        }

        public async Task<bool> IsExist(string username)
        {
            return await _accountDataManager.IsExist(username);
        }

        public async Task<(AccountModel? model, string message)> Login(AccountModel account)
        {
            if (string.IsNullOrWhiteSpace(account.Username))
                return (null, "empty:username");
            if (string.IsNullOrWhiteSpace(account.Password))
                return (null, "empty:password");

            var res = await _accountDataManager.GetAccountByUsername(account.Username);
            if (res == null)
                return (null, "not_exist");
            if (res.Password != account.Password)
                return (null, "wrong_password");
            return (res, "ok");
        }

        public async Task<(AccountModel? model, string message)> Register(AccountModel newAccount)
        {
            if (string.IsNullOrWhiteSpace(newAccount.Username))
                return (null, "empty:username");
            if (string.IsNullOrWhiteSpace(newAccount.Password))
                return (null, "empty:password");
            if (await _accountDataManager.IsExist(newAccount.Username))
                return (null, "exists");

            await _accountDataManager.PostAccount(newAccount);
            var res = await _accountDataManager.GetAccountByUsername(newAccount.Username);

            return (res, "ok");
        }
    }
}

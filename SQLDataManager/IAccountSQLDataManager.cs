using DAL.Interfaces;
using Models;

namespace SQLDataManager
{
    public class IAccountSQLDataManager : IAccountDataManager
    {
        public AccountModel? GetAccountByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string username)
        {
            throw new NotImplementedException();
        }

        public void PostAccount(AccountModel newAccount)
        {
            throw new NotImplementedException();
        }
    }
}

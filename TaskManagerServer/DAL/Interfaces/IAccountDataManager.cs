using TaskManagerServer.Models;

namespace TaskManagerServer.DAL.Interfaces
{
    public interface IAccountDataManager
    {
        AccountModel? GetAccountByUsername(string username);
        void PostAccount(AccountModel newAccount);
        bool IsExist(string username);
    }
}

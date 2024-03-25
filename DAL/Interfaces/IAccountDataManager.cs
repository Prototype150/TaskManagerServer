using Models;

namespace DAL.Interfaces
{
    public interface IAccountDataManager
    {
        Task<AccountModel?> GetAccountByUsername(string username);
        Task<bool> PostAccount(AccountModel newAccount);
        Task<bool> IsExist(string username);
    }
}

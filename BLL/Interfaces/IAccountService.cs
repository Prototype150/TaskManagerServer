
using Models;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<(AccountModel? model, string message)> Login(AccountModel account);
        Task<(AccountModel? model, string message)> Register(AccountModel newAccount);
        Task<bool> IsExist(string username);
    }
}

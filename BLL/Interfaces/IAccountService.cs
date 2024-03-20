
using Models;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        (AccountModel? model, string message) Login(AccountModel account);
        (AccountModel? model, string message) Register(AccountModel newAccount);
        bool IsExist(string username);
    }
}

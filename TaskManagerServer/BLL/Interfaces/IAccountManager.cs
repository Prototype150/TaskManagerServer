using Models;

namespace TaskManagerServer.BLL.Interfaces
{
    public interface IAccountManager
    {
        (AccountModel? model, string message) Login(AccountModel account);
        (AccountModel? model, string message) Register(AccountModel newAccount);
        bool IsExist(string username);
    }
}

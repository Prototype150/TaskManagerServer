using Models;

namespace BLL.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAccountTasks(int accountId);
        Task<(TaskModel? result, string message)> AddTask(TaskModel newTask);
        Task<(bool result, string message)> UpdateTask(TaskModel task);
        Task< (bool result, string message)> DeleteTask(int taskId);
        Task<(bool result, string message)> SwitchSortId(int accountId, int first, int second);
    }
}

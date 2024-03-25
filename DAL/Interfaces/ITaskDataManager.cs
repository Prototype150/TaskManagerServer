using Models;

namespace DAL.Interfaces
{
    public interface ITaskDataManager
    {
        Task<IEnumerable<TaskModel>> GetAccountTasks(int accountId);
        Task<TaskModel?> AddTask(TaskModel taskModel);
        Task<bool> RemoveTask(int taskId);
        Task<bool> UpdateTask(int taskId, TaskModel taskModel);
        Task<bool> IsExist(int taskId);
        Task<TaskModel?> Get(int taskId);
        Task<bool> SwitchSortId(int accountId, int firstTaskId, int secondTaskId);
    }
}

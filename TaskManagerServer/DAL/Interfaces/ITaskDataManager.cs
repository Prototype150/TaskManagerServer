using TaskManagerServer.Models;

namespace TaskManagerServer.DAL.Interfaces
{
    public interface ITaskDataManager
    {
        IEnumerable<TaskModel> GetAccountTasks(int accountId);
        bool AddTask(TaskModel taskModel);
        bool RemoveTask(int taskId);
        bool UpdateTask(int taskId, TaskModel taskModel);
        bool IsExist(int taskId);
        TaskModel? Get(int taskId);
    }
}

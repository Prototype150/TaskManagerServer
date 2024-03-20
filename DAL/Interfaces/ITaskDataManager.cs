using Models;

namespace DAL.Interfaces
{
    public interface ITaskDataManager
    {
        IEnumerable<TaskModel> GetAccountTasks(int accountId);
        TaskModel? AddTask(TaskModel taskModel);
        bool RemoveTask(int taskId);
        bool UpdateTask(int taskId, TaskModel taskModel);
        bool IsExist(int taskId);
        TaskModel? Get(int taskId);
        bool SwitchSortId(int accountId, int first, int second);
    }
}

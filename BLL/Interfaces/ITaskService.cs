using Models;

namespace BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskModel> GetAccountTasks(int accountId);
        (TaskModel? result, string message) AddTask(TaskModel newTask);
        (bool result, string message) UpdateTask(TaskModel task);
        (bool result, string message) DeleteTask(int taskId);
        (bool result, string message) SwitchSortId(int accountId, int first, int second);
    }
}

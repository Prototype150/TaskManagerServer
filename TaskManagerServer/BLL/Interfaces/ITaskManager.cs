using Models;

namespace TaskManagerServer.BLL.Interfaces
{
    public interface ITaskManager
    {
        IEnumerable<TaskModel> GetAccountTasks(int accountId);
        (bool result, string message) AddTask(TaskModel newTask);
        (bool result, string message) UpdateTask(TaskModel task);
        (bool result, string message) DeleteTask(int taskId);
        (bool result, string message) SwitchSortId(int accountId, int first, int second);
    }
}

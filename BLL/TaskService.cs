using BLL.Interfaces;
using DAL.Interfaces;
using Models;

namespace BLL
{
    public class TaskService : ITaskService
    {
        private ITaskDataManager _taskDataManager;

        public TaskService(ITaskDataManager taskDataManager)
        {
             _taskDataManager = taskDataManager;
        }

        public async Task<(TaskModel? result, string message)> AddTask(TaskModel newTask)
        {
            if (newTask.AccountId < 1)
                return (null, "id");
            if (string.IsNullOrWhiteSpace(newTask.Task))
                return (null, "empty:task");
            if (newTask.SortId > (await GetAccountTasks(newTask.AccountId)).Count())
                return (null, "out_of_bounds:task_sort_id");

            var res = await _taskDataManager.AddTask(newTask);

            if (res == null)
                return (null, "unknown_error");
            return (res, "ok");
        }

        public async Task<(bool result, string message)> DeleteTask(int taskId)
        {
            if (!await _taskDataManager.IsExist(taskId))
                return (false, "not_exist");

            var res = await _taskDataManager.RemoveTask(taskId);

            if (!res)
            {
                return (false, "unknown_error");
            }

            return (true, "ok");
        }

        public async Task<IEnumerable<TaskModel>> GetAccountTasks(int accountId)
        {
            return (await _taskDataManager.GetAccountTasks(accountId)).OrderBy(x => x.SortId);
        }

        public async Task<(bool result, string message)> SwitchSortId(int accountId, int first, int second)
        {
            var r = await _taskDataManager.SwitchSortId(accountId, first, second);
            if (!r)
                return (false, "not_exist");
            return (true, "ok");
        }

        public async Task<(bool result, string message)> UpdateTask(TaskModel task)
        {
            if (!await _taskDataManager.IsExist(task.Id))
                return (false, "not_exist");
            if (string.IsNullOrWhiteSpace(task.Task))
                return (false, "empty:task");

            bool res = await _taskDataManager.UpdateTask(task.Id, task);

            if (!res)
                return (false, "unknown_error");

            return (res, "ok");
        }
    }
}
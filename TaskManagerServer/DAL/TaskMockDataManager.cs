using TaskManagerServer.DAL.Interfaces;
using TaskManagerServer.Models;

namespace TaskManagerServer.DAL
{
    public class TaskMockDataManager : ITaskDataManager
    {
        private List<TaskModel> _tasks;
        private int counter;
        public TaskMockDataManager() {
            _tasks = new List<TaskModel>();
            counter = 0;
        }

        public bool AddTask(TaskModel taskModel)
        {
            taskModel.Id = counter++;
            _tasks.Add(taskModel);
            return true;
        }

        public IEnumerable<TaskModel> GetAccountTasks(int accountId)
        {
            return _tasks.Where(x => x.AccountId == accountId);
        }

        public bool IsExist(int taskId)
        {
            return _tasks.Any(x => x.Id == taskId);
        }

        public bool RemoveTask(int taskId)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == taskId);
            if (t != null)
                _tasks.Remove(t);
            else
                return false;
            return true;
        }

        public bool UpdateTask(int taskId, TaskModel taskModel)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == taskId);

            if(t != null)
            {
                int id = t.Id;
                t.Task = taskModel.Task;
                t.Id = id;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}

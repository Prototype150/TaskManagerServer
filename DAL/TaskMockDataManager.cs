using DAL.Interfaces;
using Models;

namespace DAL
{
    public class TaskMockDataManager : ITaskDataManager
    {
        private List<TaskModel> _tasks;
        private int counter;
        public TaskMockDataManager() {
            _tasks = new List<TaskModel>();
            counter = 0;

            AddTask(new TaskModel(0, "Zero",0, DateOnly.MinValue));
            AddTask(new TaskModel(0, "First",1, DateOnly.MinValue));
            AddTask(new TaskModel(0, "Second",2, DateOnly.MinValue));

            AddTask(new TaskModel(1, "uss",0, DateOnly.MinValue));

            AddTask(new TaskModel(2, "ass",0, DateOnly.MinValue));
            AddTask(new TaskModel(2, "no",1, DateOnly.MinValue));
            AddTask(new TaskModel(2, "yes",2, DateOnly.MinValue));
            AddTask(new TaskModel(2, "heeee",3, DateOnly.MinValue));
        }

        public async Task<TaskModel?> AddTask(TaskModel taskModel)
        {
            taskModel.Id = counter++;
            foreach(var t in _tasks.Where(x => x.AccountId == taskModel.AccountId && x.SortId >= taskModel.SortId))
            {
                t.SortId++;
            }
            _tasks.Add(taskModel);
            return taskModel;
        }

        public async Task< TaskModel?> Get(int taskId)
        {
            return _tasks.FirstOrDefault(x => x.Id == taskId);
        }

        public async Task< IEnumerable<TaskModel>> GetAccountTasks(int accountId)
        {
            return _tasks.Where(x => x.AccountId == accountId);
        }

        public async Task< bool> RemoveTask(int taskId)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == taskId);
            if (t != null)
            {
                foreach (var f in _tasks.Where(x => x.AccountId == t.AccountId && x.SortId > t.SortId))
                {
                    f.SortId--;
                }
                _tasks.Remove(t);
            }
            else
                return false;
            return true;
        }

        public async Task< bool> SwitchSortId(int accountId, int first, int second)
        {
            var f = _tasks.FirstOrDefault(x => x.AccountId == accountId && x.SortId == first);
            var s = _tasks.FirstOrDefault(x => x.AccountId == accountId && x.SortId == second);

            if (f == null || s == null)
                return false;

            int temp = s.SortId;
            s.SortId = f.SortId;
            f.SortId = temp;

            return true;
        }

        public async Task<bool> UpdateTask(int taskId, TaskModel taskModel)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == taskId);

            if(t != null)
            {
                t.Task = taskModel.Task;
                t.IsCompleted = taskModel.IsCompleted;
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsExist(int taskId)
        {
            return _tasks.Any(x => x.Id == taskId);
        }
    }
}

using Models;
using TaskManagerServer.DAL.Interfaces;

namespace TaskManagerServer.DAL
{
    public class TaskMockDataManager : ITaskDataManager
    {
        private List<TaskModel> _tasks;
        private int counter;
        public TaskMockDataManager() {
            _tasks = new List<TaskModel>();
            counter = 0;

            AddTask(new TaskModel(0, "Zero",0));
            AddTask(new TaskModel(0, "First",1));
            AddTask(new TaskModel(0, "Second",2));

            AddTask(new TaskModel(1, "uss",0));

            AddTask(new TaskModel(2, "ass",0));
            AddTask(new TaskModel(2, "no",1));
            AddTask(new TaskModel(2, "yes",2));
            AddTask(new TaskModel(2, "heeee",3));
        }

        public bool AddTask(TaskModel taskModel)
        {
            taskModel.Id = counter++;
            foreach(var t in _tasks.Where(x => x.AccountId == taskModel.AccountId && x.SortId >= taskModel.SortId))
            {
                t.SortId++;
            }
            _tasks.Add(taskModel);
            return true;
        }

        public TaskModel? Get(int taskId)
        {
            return _tasks.FirstOrDefault(x => x.Id == taskId);
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

        public bool SwitchSortId(int accountId, int first, int second)
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

        public bool UpdateTask(int taskId, TaskModel taskModel)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == taskId);

            if(t != null)
            {
                t.Task = taskModel.Task;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}

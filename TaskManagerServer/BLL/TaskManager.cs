﻿using Models;
using TaskManagerServer.BLL.Interfaces;
using TaskManagerServer.DAL.Interfaces;

namespace TaskManagerServer.BLL
{
    public class TaskManager : ITaskManager
    {
        private ITaskDataManager _taskDataManager;

        public TaskManager(ITaskDataManager taskDataManager)
        {
             _taskDataManager = taskDataManager;
        }

        public (TaskModel? result, string message) AddTask(TaskModel newTask)
        {
            if (newTask.AccountId < 0)
                return (null, "id");
            if (string.IsNullOrWhiteSpace(newTask.Task))
                return (null, "empty:task");
            if (newTask.SortId > GetAccountTasks(newTask.AccountId).Count())
                return (null, "out_of_bounds:task_id");

            var res = _taskDataManager.AddTask(newTask);
            if (res == null)
                return (null, "unknown_mistake");
            return (res, "ok");
        }


        public (bool result, string message) DeleteTask(int taskId)
        {
            if (!_taskDataManager.IsExist(taskId))
                return (false, "not_exist");

            var res = _taskDataManager.RemoveTask(taskId);

            if (!res)
            {
                return (false, "unknown_mistake");
            }

            return (res, "ok");
        }

        public IEnumerable<TaskModel> GetAccountTasks(int accountId)
        {
            return _taskDataManager.GetAccountTasks(accountId).OrderBy(x => x.SortId);
        }

        public (bool result, string message) SwitchSortId(int accountId, int first, int second)
        {
            var r = _taskDataManager.SwitchSortId(accountId, first, second);
            if (!r)
                return (false, "not_exist");
            return (true, "ok");
        }

        public (bool result, string message) UpdateTask(TaskModel task)
        {
            if (!_taskDataManager.IsExist(task.Id))
                return (false, "not_exist");
            if (string.IsNullOrWhiteSpace(task.Task))
                return (false, "empty:task");

            bool res = _taskDataManager.UpdateTask(task.Id, task);

            if (!res)
                return (false, "unknown_mistake");

            return (res, "ok");
        }
    }
}
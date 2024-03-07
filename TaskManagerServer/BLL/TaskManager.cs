﻿using TaskManagerServer.BLL.Interfaces;
using TaskManagerServer.DAL.Interfaces;
using TaskManagerServer.Models;

namespace TaskManagerServer.BLL
{
    public class TaskManager : ITaskManager
    {
        private ITaskDataManager _taskDataManager;

        public TaskManager(ITaskDataManager taskDataManager)
        {
             _taskDataManager = taskDataManager;
        }

        public (bool result, string message) AddTask(TaskModel newTask)
        {
            if (newTask.AccountId < 0)
                return (false, "id");
            if (string.IsNullOrWhiteSpace(newTask.Task))
                return (false, "empty:task");

            bool res = _taskDataManager.AddTask(newTask);
            if (!res)
                return (false, "unknown_mistake");
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
            return _taskDataManager.GetAccountTasks(accountId);
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
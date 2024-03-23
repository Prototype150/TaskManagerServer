﻿using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataManager
{
    public class TaskSQLDataManager : ITaskDataManager, IDisposable
    {
        private string _connectionString;
        private TaskManagerDbContext _dbContext;

        public TaskSQLDataManager(string connectionString) { 
            _connectionString = connectionString;
            _dbContext = new TaskManagerDbContext(_connectionString);
        }

        public TaskModel? AddTask(TaskModel taskModel)
        {
            foreach (var task in _dbContext.Tasks.Where(x => x.SortId >= taskModel.SortId && x.AccountId == taskModel.AccountId))
            {
                task.SortId++;
            }

            taskModel.Id = 0;
            var a = _dbContext.Tasks.Add(taskModel).Entity;
            _dbContext.SaveChanges();
            return a;
        }

        public TaskModel? Get(int taskId)
        {
            return _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);
        }

        public IEnumerable<TaskModel> GetAccountTasks(int accountId)
        {
            var tasks = _dbContext.Tasks.Where(x => x.AccountId == accountId);
            return tasks;
        }

        public bool IsExist(int taskId)
        {
            return _dbContext.Tasks.Any(x => x.Id == taskId);
        }

        public bool RemoveTask(int taskId)
        {
            var t = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (t == null)
                return false;

            _dbContext.Tasks.Remove(t);
            foreach (var task in _dbContext.Tasks.Where(x => x.SortId > t.SortId && x.AccountId == t.AccountId))
            {
                task.SortId--;
            }

            _dbContext.SaveChanges();
            return true;
        }

        public bool SwitchSortId(int accountId, int first, int second)
        {
            var firstTask = _dbContext.Tasks.FirstOrDefault(x => x.AccountId == accountId && x.SortId == first);
            var secondTask = _dbContext.Tasks.FirstOrDefault(x => x.AccountId == accountId && x.SortId == second);

            if (firstTask == null || secondTask == null)
                return false;

            int firstSortId = firstTask.SortId;
            firstTask.SortId = secondTask.SortId;
            secondTask.SortId = firstSortId;

            _dbContext.Tasks.Update(firstTask);
            _dbContext.Tasks.Update(secondTask);

            _dbContext.SaveChanges();

            return true;
        }

        public bool UpdateTask(int taskId, TaskModel taskModel)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                return false;

            task.Task = taskModel.Task;
            task.IsCompleted = taskModel.IsCompleted;

            _dbContext.Tasks.Update(task);
            _dbContext.SaveChanges();
            
            return true;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
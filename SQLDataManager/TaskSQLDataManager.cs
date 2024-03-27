using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataManager
{
    public class TaskSQLDataManager : ITaskDataManager
    {
        private string _connectionString;

        public TaskSQLDataManager(string connectionString) { 
            _connectionString = connectionString;
        }

        public async Task<TaskModel?> AddTask(TaskModel taskModel)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                foreach (var task in db.Tasks.Where(x => x.SortId >= taskModel.SortId && x.AccountId == taskModel.AccountId))
                {
                    task.SortId++;
                }

                taskModel.Id = 0;
                var a = (await db.Tasks.AddAsync(taskModel)).Entity;
                await db.SaveChangesAsync();
                return a;
            }
        }

        public async Task<TaskModel?> Get(int taskId)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
                return await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task<IEnumerable<TaskModel>> GetAccountTasks(int accountId)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                return db.Tasks.Where(x => x.AccountId == accountId).ToList();
            }
        }

        public async Task<bool> IsExist(int taskId)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                return await db.Tasks.AnyAsync(x => x.Id == taskId);
            }
        }

        public async Task<bool> RemoveTask(int taskId)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                var t = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                if (t == null)
                    return false;

                db.Tasks.Remove(t);
                foreach (var task in db.Tasks.Where(x => x.SortId > t.SortId && x.AccountId == t.AccountId))
                {
                    task.SortId--;
                }

                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SwitchSortId(int accountId, int firstTaskId, int secondTaskId)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                var firstTask = await db.Tasks.FirstOrDefaultAsync(x => x.Id == firstTaskId);
                var secondTask = await db.Tasks.FirstOrDefaultAsync(x => x.Id == secondTaskId);

                if (firstTask == null || secondTask == null)
                    return false;

                int firstSortId = firstTask.SortId;
                firstTask.SortId = secondTask.SortId;
                secondTask.SortId = firstSortId;

                db.Tasks.Update(firstTask);
                db.Tasks.Update(secondTask);

                await db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> UpdateTask(int taskId, TaskModel taskModel)
        {
            using (TaskManagerDbContext db = new TaskManagerDbContext(_connectionString))
            {
                var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                if (task == null)
                    return false;

                task.Task = taskModel.Task;
                task.IsCompleted = taskModel.IsCompleted;

                db.Tasks.Update(task);
                await db.SaveChangesAsync();

                return true;
            }
        }
    }
}

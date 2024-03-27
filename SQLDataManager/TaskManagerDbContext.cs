using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataManager
{
    public class TaskManagerDbContext : DbContext
    {
        private string _connectionString;
        public TaskManagerDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>().HasKey(x => x.Id);
            modelBuilder.Entity<AccountModel>().HasAlternateKey(x => x.Username);
            modelBuilder.Entity<TaskModel>().HasKey(x => x.Id);
            modelBuilder.Entity<TaskModel>().HasOne<AccountModel>().WithMany().HasForeignKey(x => x.AccountId).HasPrincipalKey(x => x.Id);
            modelBuilder.Entity<TaskModel>().HasAlternateKey(x => new { x.AccountId, x.SortId });
        }

        public bool EnshureCreation()
        {
            return Database.EnsureCreated();
        }
    }
}
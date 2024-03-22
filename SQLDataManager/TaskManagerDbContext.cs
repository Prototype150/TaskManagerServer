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
            string connectionString = "Server=DESKTOP-27FJFAJ\\EXTERMINATUS;Database=TaskManagerDatabase;Trusted_Connection=true;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>().HasKey(x => x.Id);
            modelBuilder.Entity<TaskModel>().HasKey(x => x.Id);
            modelBuilder.Entity<TaskModel>().HasOne<AccountModel>().WithMany().HasForeignKey(x => x.AccountId).HasPrincipalKey(x => x.Id);
        }
    }
}
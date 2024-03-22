using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using SQLDataManager;

string connectionString = "Server=DESKTOP-27FJFAJ\\EXTERMINATUS;Database=TaskManagerDatabase;Trusted_Connection=true;TrustServerCertificate=True";

var builder = WebApplication.CreateBuilder();

builder.WebHost.UseUrls($"https://{args[0]}:{args[1]}", $"http://{args[0]}:{args[2]}");

IAccountDataManager accountDataManager = new AccountSQLDataManager(connectionString);
accountDataManager.IsExist(null);
ITaskDataManager taskDataManager = new TaskSQLDataManager(connectionString);

builder.Services.AddSingleton(x => accountDataManager);
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton(x => taskDataManager);
builder.Services.AddSingleton<ITaskService, TaskService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
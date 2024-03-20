using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;

var builder = WebApplication.CreateBuilder();

builder.WebHost.UseUrls($"https://{args[0]}:{args[1]}", $"http://{args[0]}:{args[2]}");

builder.Services.AddSingleton<IAccountDataManager, AccountMockDataManager>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<ITaskDataManager, TaskMockDataManager>();
builder.Services.AddSingleton<ITaskService, TaskService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
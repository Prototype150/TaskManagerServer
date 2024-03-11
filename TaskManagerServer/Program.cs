using TaskManagerServer.BLL;
using TaskManagerServer.BLL.Interfaces;
using TaskManagerServer.DAL;
using TaskManagerServer.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAccountDataManager, AccountMockDataManager>();
builder.Services.AddSingleton<IAccountManager, AccountManager>();
builder.Services.AddSingleton<ITaskDataManager, TaskMockDataManager>();
builder.Services.AddSingleton<ITaskManager, TaskManager>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
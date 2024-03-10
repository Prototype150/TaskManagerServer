﻿using Models;
using System.Net;
using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace ConsoleTaskManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AccountModel? mainAccount = null;
            List<TaskModel> tasks = new List<TaskModel>();
            string commandLine = Console.ReadLine() ?? "";
            Console.Clear();
            Console.CursorVisible = false;
            int inputLine = 0;
            bool isTaskListDisplayed = false;

            while (commandLine != "stop")
            {
                var res = commandLine.Split(' ', StringSplitOptions.TrimEntries);
                int commandLegth = res.Length;

                if (commandLegth == 0)
                {
                    Console.CursorTop = inputLine;
                    continue;
                }

                string mainCommand = res[0];
                if (mainCommand == "register")
                {
                    if (res.Length != 3)
                    {
                        
                    }
                    else
                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri("https://localhost:7025/account/register")) { Content = new StringContent(JsonSerializer.Serialize<AccountModel>(new AccountModel(res[1], res[2])), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var stringResponce = await responce.Content.ReadAsStringAsync();
                                mainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                Console.Clear();
                                Console.Write("Register successfull!");

                                int a = Console.BufferWidth - ("Username: " + mainAccount.Username).Length;
                                int b = Console.BufferWidth - ("Password: " + mainAccount.Password).Length;

                                Console.CursorLeft = a;
                                Console.WriteLine("Username: " + mainAccount.Username);
                                Console.CursorLeft = b;
                                Console.WriteLine("Password: " + mainAccount.Password);
                                Console.CursorLeft = 0;
                                inputLine = 1;
                                isTaskListDisplayed = false;
                            }
                            else if (responce.StatusCode == HttpStatusCode.BadRequest)
                            {

                            }
                        }
                }

                else if (mainCommand == "login")
                {
                    if (res.Length != 3)
                    {

                    }
                    else
                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:7025/account/login")) { Content = new StringContent(JsonSerializer.Serialize<AccountModel>(new AccountModel(res[1], res[2])), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var stringResponce = await responce.Content.ReadAsStringAsync();
                                mainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                Console.Clear();
                                Console.Write("Login successfull!");

                                int a = Console.BufferWidth - ("Username: " + mainAccount.Username).Length;
                                int b = Console.BufferWidth - ("Password: " + mainAccount.Password).Length;

                                Console.CursorLeft = a;
                                Console.WriteLine("Username: " + mainAccount.Username);
                                Console.CursorLeft = b;
                                Console.WriteLine("Password: " + mainAccount.Password);
                                Console.CursorLeft = 0;
                                inputLine = 1;
                                isTaskListDisplayed = false;
                            }
                            else if(responce.StatusCode == HttpStatusCode.BadRequest)
                            {

                            }
                        }
                }

                else if(mainCommand == "tasks")
                {
                    if (res.Length != 1 || mainAccount == null)
                    {

                    }
                    else
                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:7025/task/" + mainAccount.Id));
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var stringResponce = await responce.Content.ReadAsStringAsync();
                                tasks = JsonSerializer.Deserialize<List<TaskModel>>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? tasks;
                                Console.CursorTop = 1;
                                Console.CursorLeft = 0;

                                for (int i = 0; i < tasks.Count; i++)
                                {
                                    Console.WriteLine(i+1 + "." + tasks[i].Task);
                                }

                                inputLine = tasks.Count + 1;
                                isTaskListDisplayed = true;
                            }
                        }
                }

                else if(mainCommand == "add")
                {
                    if (res.Length != 2 || mainAccount == null || !isTaskListDisplayed)
                    {

                    }
                    else
                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri("https://localhost:7025/task/add")) { Content = new StringContent(JsonSerializer.Serialize<TaskModel>(new TaskModel(mainAccount.Id, res[1], tasks.Count)), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                Console.WriteLine(res[1]);
                                inputLine++;
                            }
                        }
                }

                else if(mainCommand == "update")
                {
                    if (res.Length != 3 || mainAccount == null || !isTaskListDisplayed)
                    {

                    }
                    else
                        using (HttpClient client = new HttpClient())
                        {
                            int index = Convert.ToInt32(res[1]) - 1;
                            int howMuchToClean = tasks[index].Task.Length + 2;
                            tasks[index].Task = res[2];
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, new Uri("https://localhost:7025/task/update")) { Content = new StringContent(JsonSerializer.Serialize<TaskModel>(tasks[index]), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var a = await responce.Content.ReadAsStringAsync();
                                var iS = JsonSerializer.Deserialize<bool>(a);
                                if (iS)
                                {
                                    Console.CursorLeft = 0;
                                    Console.CursorTop = index + 1;
                                    Console.Write(new String(' ', howMuchToClean));
                                    Console.CursorLeft = 0;
                                    Console.Write(index + 1 + "." + tasks[index].Task);
                                }
                            }
                        }
                }

                Console.CursorLeft = 0;
                Console.CursorTop = inputLine;
                commandLine = Console.ReadLine() ?? "";
                Console.CursorTop = inputLine;
                Console.Write(new String(' ', commandLine.Length));
                Console.CursorLeft = 0;
            }
        }
    }
}
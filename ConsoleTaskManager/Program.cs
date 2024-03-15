using Models;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace ConsoleTaskManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            int lastMessageLength = 0;

            string url = "https://26.218.3.87:7025";
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
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri(url+"/account/register")) { Content = new StringContent(JsonSerializer.Serialize<AccountModel>(new AccountModel(res[1], res[2])), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var stringResponce = await responce.Content.ReadAsStringAsync();
                                mainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                Console.Clear();
                                lastMessageLength = "Register successful!".Length;
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
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(url + "/account/login")) { Content = new StringContent(JsonSerializer.Serialize<AccountModel>(new AccountModel(res[1], res[2])), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var stringResponce = await responce.Content.ReadAsStringAsync();
                                mainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                Console.Clear();
                                lastMessageLength = "Login successful!".Length;
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
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(url + "/task/" + mainAccount.Id));
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
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            var newTask = new TaskModel(mainAccount.Id, res[1], tasks.Count);
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri(url + "/task/add")) { Content = new StringContent(JsonSerializer.Serialize<TaskModel>(newTask), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                var a = await responce.Content.ReadAsStringAsync();
                                newTask = JsonSerializer.Deserialize<TaskModel>(a, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                tasks.Add(newTask);
                                Console.WriteLine(inputLine+"."+res[1]);
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
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            int index = Convert.ToInt32(res[1]) - 1;
                            var updatedTask = new TaskModel(tasks[index].AccountId, res[2], tasks[index].SortId) { Id = tasks[index].Id };
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, new Uri(url + "/task/update")) { Content = new StringContent(JsonSerializer.Serialize<TaskModel>(updatedTask), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            var responce = await client.SendAsync(message);
                            if(responce.IsSuccessStatusCode)
                            {
                                string sResult = await responce.Content.ReadAsStringAsync();
                                bool iS = JsonSerializer.Deserialize<bool>(sResult);
                                if (iS)
                                {
                                    Console.CursorTop = index + 1;
                                    Console.CursorLeft = 0;

                                    Console.Write(new String(' ', tasks[index].Task.Length + 2));
                                    Console.CursorLeft = 0;
                                    Console.Write(index + 1 + "." + res[2]);
                                }
                                tasks[index].Task = res[2];
                            }
                        }
                }

                else if(mainCommand == "delete")
                {
                    if(res.Length != 2 || mainAccount == null || !isTaskListDisplayed)
                    {

                    }
                    else 
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            int index = Convert.ToInt32(res[1]) - 1;
                            int trueIndex = index;
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, new Uri(url + "/task/delete/" + tasks[index].Id));
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                int cleanL = tasks[index].Task.Length + 2;
                                var a = await responce.Content.ReadAsStringAsync();
                                var iS = JsonSerializer.Deserialize<bool>(a);
                                if (iS)
                                {
                                    Console.CursorTop = index + 1;
                                    for (; index < tasks.Count - 1; index++)
                                    {
                                        Console.CursorTop = index + 1;
                                        Console.CursorLeft = 0;
                                        Console.Write(new String(' ', tasks[index].Task.Length + 2));
                                        Console.CursorLeft = 0;
                                        Console.WriteLine(index + 1 + "." + tasks[index + 1].Task);
                                    }
                                    Console.Write(new String(' ', tasks[index].Task.Length + 2));
                                    Console.CursorLeft = 0;

                                    inputLine--;
                                    tasks.RemoveAt(trueIndex);
                                    for (int i = trueIndex; i < tasks.Count; i++)
                                    {
                                        tasks[i].SortId--;
                                    }
                                }
                            }
                        }
                }

                else if(mainCommand == "switch")
                {
                    if(res.Length != 3 || mainAccount == null || !isTaskListDisplayed)
                    {

                    }
                    else
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            int realFirstIndex = Convert.ToInt32(res[1]);
                            int realSecondIndex = Convert.ToInt32(res[2]);
                            int first = tasks[Convert.ToInt32(res[1]) - 1].SortId;
                            int second = tasks[Convert.ToInt32(res[2]) - 1].SortId;

                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put,new Uri($"{url}/task/switch/{mainAccount.Id}/{first}/{second}"));
                            var responce = await client.SendAsync(message);
                            if (responce.IsSuccessStatusCode)
                            {
                                string sResult = await responce.Content.ReadAsStringAsync();
                                bool iS = JsonSerializer.Deserialize<bool>(sResult);
                                if (iS)
                                {
                                    Console.CursorTop = realFirstIndex;
                                    Console.CursorLeft = 0;
                                    Console.Write(new String(' ', tasks[realFirstIndex - 1].Task.Length + 2));
                                    Console.CursorLeft = 0;
                                    Console.Write(realFirstIndex + "." + tasks[realSecondIndex - 1].Task);
                                    Console.CursorLeft = 0;
                                    Console.CursorTop = realSecondIndex;
                                    Console.Write(new String(' ', tasks[realSecondIndex - 1].Task.Length + 2));
                                    Console.CursorLeft = 0;
                                    Console.Write(realSecondIndex + "." + tasks[realFirstIndex - 1].Task);

                                    TaskModel temp = tasks[realFirstIndex - 1];
                                    tasks[realFirstIndex - 1] = tasks[realSecondIndex - 1];
                                    tasks[realSecondIndex - 1] = temp;

                                    int tempSortId = tasks[realFirstIndex - 1].SortId;
                                    tasks[realFirstIndex - 1].SortId = tasks[realSecondIndex - 1].SortId;
                                    tasks[realSecondIndex - 1].SortId = tempSortId;
                                }
                            }
                        }
                }
                clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

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
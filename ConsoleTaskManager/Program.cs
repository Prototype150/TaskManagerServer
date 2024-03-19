using ConsoleTaskManager.Command;
using ConsoleTaskManager.Command.Interfaces;
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
        private static string _mainMessage = "";
        public static string MainMessage
        {
            get { return _mainMessage; }
            set 
            {
                Clean(0, 0, _mainMessage.Length);
                WriteAt(0, 0, value);
                _mainMessage = value;
            }
        }
        private static AccountModel? MainAccount = null;
        private static ICommandChecker commandChecker;

        public static async Task Main(string[] args)
        {
            commandChecker = new CommandChecker();

            string connectionString = $"https://{args[0]}:{args[1]}";
            string commandLine = (Console.ReadLine() ?? "").Trim();

            int inputLine = 0;

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            while (commandLine != "stop")
            {
                string[] commandSplit = commandLine.Split(' ', StringSplitOptions.TrimEntries);

                if (commandSplit[0] == "register")
                {
                    if (commandChecker.IsCommandCorrect(CommandType.Register, commandSplit))
                    {
                        using (HttpClient client = new HttpClient(handler))
                        {
                            AccountModel newAccount = new AccountModel(commandSplit[1], commandSplit[2]);
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri(connectionString + "/account/register")) { Content = new StringContent(JsonSerializer.Serialize(newAccount), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            HttpResponseMessage responce = await client.SendAsync(message);
                            string stringResponce = await responce.Content.ReadAsStringAsync();

                            if (responce.IsSuccessStatusCode)
                            {
                                MainMessage = "Registered succesful!";
                                MainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                inputLine = 1;
                            }
                            else
                            {
                                MainMessage = stringResponce;
                            }
                        }
                    }
                    else
                    {
                        MainMessage = "Incorrect \"register\" command parameters.";
                    }
                }
                else if (commandSplit[0] == "login")
                {
                    if (commandChecker.IsCommandCorrect(CommandType.Login, commandSplit))
                    {
                        using (HttpClient client = new HttpClient(handler))
                        {
                            AccountModel accountData = new AccountModel(commandSplit[1], commandSplit[2]);
                            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(connectionString + "/account/login")) { Content = new StringContent(JsonSerializer.Serialize(accountData), Encoding.UTF8, MediaTypeNames.Application.Json) };
                            HttpResponseMessage responce = await client.SendAsync(message);
                            string stringResponce = await responce.Content.ReadAsStringAsync();

                            if (responce.IsSuccessStatusCode)
                            {
                                MainMessage = "Login succesful!";
                                MainAccount = JsonSerializer.Deserialize<AccountModel>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                inputLine = 1;
                            }
                            else
                            {
                                MainMessage = stringResponce;
                            }
                        }
                    }
                    else
                    {
                        MainMessage = "Incorrect \"login\" command parameters.";
                    }
                }
                else if (commandSplit[0] == "tasks")
                {
                    if (commandChecker.IsCommandCorrect(CommandType.GetTasks, commandSplit))
                    {
                        if (MainAccount != null)
                        {
                            using (HttpClient client = new HttpClient(handler))
                            {
                                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(connectionString + "/task/" + MainAccount.Id));
                                HttpResponseMessage responce = await client.SendAsync(message);
                                string stringResponce = await responce.Content.ReadAsStringAsync();

                                if (responce.IsSuccessStatusCode)
                                {
                                    MainMessage = "Task list displayed!";
                                    IEnumerable<TaskModel> tasks = JsonSerializer.Deserialize<IEnumerable<TaskModel>>(stringResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                }
                                else
                                {
                                    MainMessage = stringResponce;
                                }
                            }
                        }
                        else
                        {
                            MainMessage = "Can't retriew tasks. User is not logged in";
                        }
                    }
                    else
                    {
                        MainMessage = "Incorrect \"tasks\" command parameters";
                    }
                }
                else
                {
                    MainMessage = "Wrong command!";
                }

                handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, ss) => { return true; };

                SetConsoleCursor(0, inputLine);

                commandLine = (Console.ReadLine() ?? "").Trim();
                Clean(0, inputLine, commandLine.Count());
            }

            handler.Dispose();
        }

        public static void SetConsoleCursor(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }

        public static void Clean(int x, int y, int length)
        {
            (int x, int y) originalCoordinates = (Console.CursorLeft, Console.CursorTop);

            Console.CursorLeft = x; 
            Console.CursorTop = y;

            Console.Write(new String(' ', length));

            Console.CursorLeft = originalCoordinates.x;
            Console.CursorTop = originalCoordinates.y;
        }

        public static void WriteAt(int x, int y, string message)
        {
            (int x, int y) originalCoordinates = (Console.CursorLeft, Console.CursorTop);

            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(message);

            Console.CursorLeft = originalCoordinates.x;
            Console.CursorTop = originalCoordinates.y;
        }
    }
}
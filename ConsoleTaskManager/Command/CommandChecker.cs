using ConsoleTaskManager.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager.Command
{
    public class CommandChecker : ICommandChecker
    {
        public bool IsCommandCorrect(CommandType commandType, IEnumerable<string> commandWithParams)
        {
            if(commandType == CommandType.Register)
            {
                if (commandWithParams == null)
                    return false;
                if(commandWithParams.Count() != 3)
                    return false;
                if (string.IsNullOrWhiteSpace(commandWithParams.ElementAt(0)) || string.IsNullOrWhiteSpace(commandWithParams.ElementAt(1)) || string.IsNullOrWhiteSpace(commandWithParams.ElementAt(2)))
                    return false;
                if (commandWithParams.ElementAt(0) != "register")
                    return false;
                return true;
            }
            else if(commandType == CommandType.Login)
            {
                if (commandWithParams == null)
                    return false;
                if (commandWithParams.Count() != 3)
                    return false;
                if (string.IsNullOrWhiteSpace(commandWithParams.ElementAt(0)) || string.IsNullOrWhiteSpace(commandWithParams.ElementAt(1)) || string.IsNullOrWhiteSpace(commandWithParams.ElementAt(2)))
                    return false;
                if (commandWithParams.ElementAt(0) != "login")
                    return false;
                return true;
            }
            else if(commandType == CommandType.GetTasks)
            {
                if (commandWithParams == null)
                    return false;
                if (commandWithParams.Count() != 1)
                    return false;
                if (commandWithParams.ElementAt(0) != "tasks")
                    return false;
                return true;
            }
            return false;
        }
    }
}

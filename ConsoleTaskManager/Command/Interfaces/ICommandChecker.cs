using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskManager.Command.Interfaces
{
    public enum CommandType
    {
        Register,
        Login,
        GetTasks
    }

    public interface ICommandChecker
    {
        bool IsCommandCorrect(CommandType commandType, IEnumerable<string> commandWithParams);
    }
}

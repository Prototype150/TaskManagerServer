using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace TaskManagerServer.Models.Filters
{
    public class RegisterFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var f = context.Result as ObjectResult;
        }
    }
}

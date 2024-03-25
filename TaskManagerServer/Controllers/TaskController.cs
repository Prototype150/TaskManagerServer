using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using BLL.Interfaces;
using TaskManagerServer.Models.Validations;

namespace TaskManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskManager;

        public TaskController(ITaskService taskManager) {
            _taskManager = taskManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{accountId}")]
        public async Task<IActionResult> GetAccountTasks(int accountId)
        {
            var a = await _taskManager.GetAccountTasks(accountId);
            return Ok(a);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddTask([FromBody][TaskValidation]TaskModel taskModel)
        {
            var res = await _taskManager.AddTask(taskModel);

            if (res.result == null)
            {
                return BadRequest(res.message);
            }

            return Ok(res.result);
        }

        [HttpPut]
        [Route("switch/{accountId}/{firstId}/{secondId}")]
        public async Task<IActionResult> SwitchTasks(int accountId,int firstId, int secondId)
        {
            var res = await _taskManager.SwitchSortId(accountId, firstId, secondId);
            if (!res.result)
            {
                return BadRequest(res.message);
            }
            
            return Ok(res.result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateTask([FromBody][TaskValidation]TaskModel taskModel)
        {

            var res = await _taskManager.UpdateTask(taskModel);

            if (!res.result)
            {
                return BadRequest(res.message);
            }

            Console.WriteLine("Successfuly updated task");
            return Ok(res.result);
        }

        [HttpDelete]
        [Route("delete/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var res = await _taskManager.DeleteTask(taskId);

            return Ok(res.result);
        } 
    }
}
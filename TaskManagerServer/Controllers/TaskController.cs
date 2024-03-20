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
        public IActionResult GetAccountTasks(int accountId)
        {
            return Ok(_taskManager.GetAccountTasks(accountId));
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask([FromBody][TaskValidation]TaskModel taskModel)
        {
            var res = _taskManager.AddTask(taskModel);

            if (res.result == null)
            {
                return BadRequest(res.message);
            }

            return Ok(res.result);
        }

        [HttpPut]
        [Route("switch/{accountId}/{firstId}/{secondId}")]
        public IActionResult SwitchTasks(int accountId,int firstId, int secondId)
        {
            var res = _taskManager.SwitchSortId(accountId, firstId, secondId);
            if (!res.result)
            {
                return BadRequest(res.message);
            }
            
            return Ok(res.result);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateTask([FromBody][TaskValidation]TaskModel taskModel)
        {
            var res = _taskManager.UpdateTask(taskModel);

            if (!res.result)
            {
                return BadRequest(res.message);
            }

            return Ok(res.result);
        }

        [HttpDelete]
        [Route("delete/{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            var res = _taskManager.DeleteTask(taskId);

            return Ok(res.result);
        } 
    }
}
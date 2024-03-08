using Microsoft.AspNetCore.Mvc;
using TaskManagerServer.BLL.Interfaces;
using TaskManagerServer.Models;

namespace TaskManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskManager _taskManager;

        public TaskController(ITaskManager taskManager) {
            _taskManager = taskManager;
        }

        [HttpGet]
        [Route("{accountId}")]
        public IActionResult GetAccountTasks(int accountId)
        {
            return Ok(_taskManager.GetAccountTasks(accountId));
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask(TaskModel taskModel)
        {
            var res = _taskManager.AddTask(taskModel);

            if (!res.result)
            {
                return BadRequest(res.message);
            }

            return Ok(true);
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
            
            return Ok(true);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateTask([FromBody]TaskModel taskModel)
        {
            var res = _taskManager.UpdateTask(taskModel);

            if (!res.result)
            {
                return BadRequest(res.message);
            }

            return Ok(true);
        }

        [HttpDelete]
        [Route("delete/{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            _taskManager.DeleteTask(taskId);

            return Ok(true);
        }
    }
}
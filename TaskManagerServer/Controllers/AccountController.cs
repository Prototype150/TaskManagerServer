using Microsoft.AspNetCore.Mvc;
using TaskManagerServer.BLL;
using TaskManagerServer.BLL.Interfaces;
using TaskManagerServer.Models;
using TaskManagerServer.Models.Filters;
using TaskManagerServer.Models.Validations;

namespace TaskManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountManager _accountManager;
        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody]AccountModel newAccount)
        {
            (AccountModel? account, string message) res;
            res = _accountManager.Register(newAccount);
            if (res.account == null)
            {
                return BadRequest(res.message);
            }

            return Ok(res.account);
        }


        [HttpGet]
        [Route("login")]
        public IActionResult Login([FromBody]AccountModel account)
        {
            (AccountModel? account, string message) res;
            res = _accountManager.Login(account);
            if (res.account == null)
            {
                return BadRequest(res.message);
            }

            return Ok(res.account);
        }
    }
}

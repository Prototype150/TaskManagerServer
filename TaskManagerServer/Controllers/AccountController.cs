using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Principal;
using TaskManagerServer.BLL.Interfaces;
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
            if (string.IsNullOrWhiteSpace(newAccount.Username))
                return BadRequest("empty:username");
            if (string.IsNullOrWhiteSpace(newAccount.Username))
                return BadRequest("empty:password");
            (AccountModel? account, string message) res = _accountManager.Register(newAccount);
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
            if (string.IsNullOrWhiteSpace(account.Username))
                return BadRequest("empty:username");
            if (string.IsNullOrWhiteSpace(account.Password))
                return BadRequest("empty:password");
            (AccountModel? account, string message) res = _accountManager.Login(account);
            if (res.account == null)
            {
                if (res.message == "not_exist")
                    return NotFound(res.message);
                return BadRequest(res.message);
            }

            return Ok(res.account);
        }
    }
}

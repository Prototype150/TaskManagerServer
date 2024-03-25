using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Principal;
using TaskManagerServer.Models.Validations;

namespace TaskManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountManager;
        public AccountController(IAccountService accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]AccountModel newAccount)
        {
            if (string.IsNullOrWhiteSpace(newAccount.Username))
                return BadRequest("empty:username");
            if (string.IsNullOrWhiteSpace(newAccount.Username))
                return BadRequest("empty:password");
            (AccountModel? account, string message) res = await _accountManager.Register(newAccount);
            if (res.account == null)
            {
                return BadRequest(res.message);
            }

            return Ok(res.account);
        }


        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]AccountModel account)
        {
            if (string.IsNullOrWhiteSpace(account.Username))
                return BadRequest("empty:username");
            if (string.IsNullOrWhiteSpace(account.Password))
                return BadRequest("empty:password");
            (AccountModel? account, string message) res = await _accountManager.Login(account);
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

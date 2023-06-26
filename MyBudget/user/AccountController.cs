using Microsoft.AspNetCore.Mvc;
using MyBudget.user.dtos;

namespace MyBudget.user
{
    public interface IUserController
    {
        ActionResult Register([FromBody] RegisterUserDto userDto);
    }

    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase, IUserController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto userDto)
        {
            _accountService.Register(userDto);
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            string token = _accountService.GenereteJwt(loginDto);
            return Ok(token);
        }
    }
}

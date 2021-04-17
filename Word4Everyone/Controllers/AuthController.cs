using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Word4Everyone.Model;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        //private IMailService _mailService;

        public AuthController(IUserService userService, IMailService mailService)
        {
            _userService = userService;
            //_mailService = mailService;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); //Status code: 200

                return BadRequest(result);
            }

            // Ошибка на клиенте
            return BadRequest("Введены неверные данные"); // Status code: 400 
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    //await _mailService.SendEmailAsync(model.Email, "New Login", $"<h1>Hey! New login to your account noticed at {DateTime.Now}</h1>");
                    return Ok(result); //Status code: 200
                }

                return Unauthorized(result);
            }

            return Unauthorized("Введены неверные данные"); // Status code: 400 
        }

        // GET: api/Auth/ConfirmEmail
        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        //        return NotFound();

        //    var result = await _userService.ConfirmEmailAsync(userId, token);

        //    if (result.IsSuccess)
        //    {
        //        return Ok();
        //        //return Redirect($"{_configuration["thisAppUrl"]}/ConfirmEmail.html")
        //    };

        //    return BadRequest(result);
        //}
    }
}

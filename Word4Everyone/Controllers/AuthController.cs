using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Word4Everyone.Model;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public AuthController(IUserService userService, IMailService mailService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _userService = userService;
            _mailService = mailService;
            _sharedLocalizer = sharedLocalizer;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            // TODO: Do I really need this, if model checks itself automatically
            // if (!ModelState.IsValid) 
            //    return BadRequest(_sharedLocalizer["WrongRegisterModel"].Value);

            UserManagerResponse result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess)
                return Ok(result);
            
            return BadRequest(result);
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            // TODO
            // if (!ModelState.IsValid) 
            //    return BadRequest(_sharedLocalizer["WrongLoginModel"].Value);

            UserManagerResponse result = await _userService.LoginUserAsync(model);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        // GET: api/Auth/ConfirmEmail
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            UserManagerResponse result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
                return Ok(_sharedLocalizer["EmailConfirmed"].Value);
            
            return BadRequest(result);
        }

        // GET: api/Auth/ForgetPassword
        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound(_sharedLocalizer["NoEmail"].Value);

            UserManagerResponse result = await _userService.ForgetPasswordAsync(email);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        // POST: api/Auth/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model) //FromForm
        {
            //TODO
            // if (!ModelState.IsValid) return BadRequest();

            UserManagerResponse result = await _userService.ResetPasswordAsync(model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);

        }
    }
}

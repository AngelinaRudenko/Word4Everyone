using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Word4Everyone.Services;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //[Authorize] - to allow authorized users
        //You can also put this attribute before the class difinition
        //[AllowAnonymus] - to allow access for not authorized users
        //This attribute always overrides [Authorized]
        //For example if class is [AllowAnonymus], but method is [Authorized], 
        //user will have access to method without authorization

        private IUserService _userService;
        private IMailService _mailService;

        public AuthController(IUserService userService, IMailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
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

            // Smth wrong from the client side
            return BadRequest("Some properties are not valid"); // Status code: 400 
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
                    await _mailService.SendEmailAsync(model.Email, "New Login", $"<h1>Hey! New login to your account noticed at {DateTime.Now}</h1>");
                    return Ok(result); //Status code: 200
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400 
        }

        // GET: api/Auth/ConfirmEmail
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Ok();
                //return Redirect($"{_configuration["thisAppUrl"]}/ConfirmEmail.html")
            };

            return BadRequest(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Word4Everyone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckIfAuthorizedController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            return Ok($"Вы авторизованы!\nВаш id {userId}");
        }
    }
}

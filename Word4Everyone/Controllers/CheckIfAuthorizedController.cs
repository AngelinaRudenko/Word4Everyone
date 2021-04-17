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
        //[Authorize] - to allow authorized users
        //You can also put this attribute before the class difinition
        //[AllowAnonymus] - to allow access for not authorized users
        //This attribute always overrides [Authorized]
        //For example if class is [AllowAnonymus], but method is [Authorized], 
        //user will have access to method without authorization

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            return Ok($"Вы авторизованы!\nВаш id {userId}");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            return Ok($"You are authorized!\nYours id {userId}");
        }
    }
}

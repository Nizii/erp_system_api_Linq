using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
namespace WebApplication1.Controllers
{
 [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(IConfiguration configuration, IWebHostEnvironment env,
       IMemoryCache cache)
        {
        }
        [HttpGet("{username}/{password}")]
        public JsonResult Get(string username, string password)
        {
            return new JsonResult(" ");
        }
    }
}
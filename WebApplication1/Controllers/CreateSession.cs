
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Http;
using ErpSystemDbContext;
using User = ErpSystemDbContext.User;

namespace WebApplication1
{
    [Route("api")]
    public class TestController : Controller
    {
        public TestController(){}

        [HttpGet]
        [Route("{username}/{password}")]
        public JsonResult Get(string username, string password)
        {
            User user = GetUserFromDB(username, password);
            if (user != null)
            {
                HttpContext.Session.Set<User>(username, user);
                return new JsonResult(user.UserName);
            }
            return new JsonResult(null);
        }

        [HttpPost]
        [Route("{username}/{password}")]
        public JsonResult Post(string username, string password)
        {
            // ToDo: ->  look if user already exists
            int salt = 12;
            User user = null;
            user.UserName = username;
            user.UserPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            // ToDo: ->  Insert with linq
            HttpContext.Session.Set<User>(username, user);
            return new JsonResult(user.UserName);
        }

        [HttpGet]
        [Route("get")]
        public JsonResult Get()
        {
            User user = HttpContext.Session.Get<User>("Nizam");
            return new JsonResult($"{user.UserName}, info fetched from session");
        }

        [HttpGet]
        [Route("clear")]
        public JsonResult ClearSession()
        {
            HttpContext.Session.Clear();
            return new JsonResult($"session clear");
        }

        protected User GetUserFromDB(string username, string password)
        {
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            User user = model.Users.Where(user => user.UserName == username).FirstOrDefault();
            //if (BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
            if(user.UserPassword == password)
                return user;
            return null;
        }
    }
}
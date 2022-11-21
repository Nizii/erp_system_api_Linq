using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using ErpSystemDbContext;
using User = ErpSystemDbContext.User;
using WebApplication1;
using System.Diagnostics;

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
            Debug.WriteLine("Get Request has been triggered");
            User user = GetUser(username, password);
            Debug.Write("Login "+user.UserName);
            if (user != null)
            {
                HttpContext.Session.Set<User>(username, user);
                Debug.Write("Login Succeed");
                return new JsonResult($"{user.UserName}, save to session");
            }
            else
            {
                Debug.Write("Login failed");
                return new JsonResult(null);
            }
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

        protected User GetUser(string username, string password)
        {
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            User user = model.Users.Where(user => user.UserName == username).FirstOrDefault();
            //if (BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
            if(user.UserPassword == password)
            {
                Debug.WriteLine("Password korrekt");
                return user;
            }
            Debug.WriteLine("Password inkorrekt");
            return null;
        }
    }
}
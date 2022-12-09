
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Http;
using ErpSystemDbContext;
using User = ErpSystemDbContext.User;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Security.Cryptography;
using Devart.Data.Linq;

namespace WebApplication1
{
    [Route("api")]
    [EnableCors("AllowOrigin")]
    public class CreateSession : ControllerBase
    {

        [HttpGet]
        [Route("{username}/{password}")]
        public JsonResult Get(string username, string password)
        {
            User user = GetUserFromDB(username, password);
            if (user != null)
            {
                Debug.WriteLine("User CreateSession " + user.UserName);
                HttpContext.Session.Set("TestUser", user);
                return new JsonResult(user.UserName);
            }
            return new JsonResult(null);
        }

        [HttpPost]
        [Route("{username}/{password}")]
        public JsonResult Post(string username, string password)
        {
            if(!GetUserNameFromDB(username))
                return new JsonResult(null);
            int salt = 12;
            User user = new User
            {
                UserName = username,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(password, salt)
            };
            InsertNewUser(user);
            HttpContext.Session.Set(username, user);
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
            if (BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
                return user;
            return null;
        }

        protected bool GetUserNameFromDB(string username)
        {
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            return model.Users.Where(user => user.UserName == username).FirstOrDefault() is null;
        }

        protected void InsertNewUser(User user)
        {
            try
            {
                ErpSystemDbDataContext model = new ErpSystemDbDataContext();
                model.Users.InsertOnSubmit(user);
                model.SubmitChanges();
            }
            catch (Exception ex) { 
                Debug.WriteLine("Insert failed with Ex: " + ex);
            }
        }
    }
}
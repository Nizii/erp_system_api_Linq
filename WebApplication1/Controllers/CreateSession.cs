using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionMvc.App.Utilities;
using WebApplication1.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace SessionMvc.App.Controllers
{

    [Route("session")]
    public class TestController : Controller
    {
        protected MySqlConnection con;
        protected readonly IConfiguration _configuration;
        public TestController()
        {
            string connStr = _configuration.GetConnectionString("ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }


        [HttpGet]
        //[Route("{username}/{password}")]
        [Route("set")]
        public IActionResult SaveToSession(string username, string password)
        {
            var user = GetUser("Nizam", "123456");
 
            if(user != null)
            {
                HttpContext.Session.Set<User>("info", user);
                return Content($"save to session");
            } 
            else
            {
                return Content($"No User found");
            }
        }

        [HttpGet]
        [Route("get")]
        public IActionResult FetchFromSession()
        {
            User info = HttpContext.Session.Get<User>("info");
            return Content($"{info.user_name} info fetched from session");
        }

        [HttpGet]
        [Route("clear")]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return Content($"session clear");
        }

        protected User GetUser(string username, string password)
        {
            User user = null;
            try
            {
                string connStr = _configuration.GetConnectionString("ConnectionStringForDatabase");
                con = new MySqlConnection(connStr);
                con.Open();
                var cmd = new MySqlCommand("SELECT user_name, user_password from erp_system_db.user where user_name ='" + username + "' and user_password ='" + password + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        user_nr = (int)reader["user_nr"],
                        user_name = reader["user_name"].ToString(),
                        user_email = reader["user_email"].ToString(),
                        user_password = reader["user_password"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            Debug.WriteLine("User Nr "+user.user_nr);
            con.Close();
            return user;
        }
    }
}
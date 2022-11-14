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
        string tempKey = "Nizam";
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
        public JsonResult Set(string username, string password)
        {
            //var user = GetUser("Nizam", "123456");
            
            var user = new User
            {
                user_nr = 1,
                user_name = "Nizam",
            };

            if (user != null)
            {
                HttpContext.Session.Set<User>(tempKey, user);
                return new JsonResult($"{user.user_name}, save to session");
            }
            else
            {
                return new JsonResult($"No User found");
            }
        }

        [HttpGet]
        [Route("get")]
        public JsonResult Get()
        {
            User user = HttpContext.Session.Get<User>(tempKey);
            return new JsonResult($"{user.user_name}, info fetched from session");
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
            Debug.WriteLine("User Nr " + user.user_nr);
            con.Close();
            return user;
        }
    }
}
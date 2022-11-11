using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using WebApplication1.Models;
using System.Diagnostics;
using BCrypt.Net;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public UserController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
            string connStr = ConfigurationExtensions.GetConnectionString(_configuration, "ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }

        /*
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            return new JsonResult(dbList);
        }
        */

        [HttpGet]
        public string[] Get([FromQuery] User user)
        {
            /*
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string[] result_array = new string[3];
            foreach (var result in dbList)
            {
                if(user.user_name.Equals(result.user_name.ToString())) {

                    if (BCrypt.Net.BCrypt.Verify(user.user_password, result.user_password)) {
                        result_array[0] = "Login Succeed";
                        result_array[1] = result.user_nr.ToString();
                        result_array[2] = result.user_name;
                        break;
                    }
                    result_array[0] = "Password incorrect";
                    break;
                } 
                else
                {
                    result_array[0] = "User not found";
                }
            }
            */
            return null;
        }


        [HttpPost]
        public string[] Post([FromQuery] User user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string[] result_array = new string[3];
            bool name_is_not_double = true;
            
            foreach (var user_from_db in dbList)
            {
                if (user.user_name.Equals(user_from_db.user_name))
                {
                    result_array[0] = "Username already exists";
                    name_is_not_double = false; 
                    break;
                }
            }

            if (name_is_not_double)
            {
                int salt = 12;
                user.user_password = BCrypt.Net.BCrypt.HashPassword(user.user_password, salt);

                int lastUserId = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable().Count();
                user.user_nr = lastUserId + 1;
                dbClient.GetDatabase("Database").GetCollection<User>("User").InsertOne(user);
                result_array[0] = "Sign Up Succeed";
                result_array[1] = user.user_nr.ToString();
                result_array[2] = user.user_name;
            }

            return result_array;
        }

       
        [HttpPut]
        public JsonResult Put(User user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<User>.Filter.Eq("user_nr", user.user_nr);
            var update = Builders<User>.Update.Set("user_name", user.user_name)
                                                    .Set("user_email", user.user_email)
                                                    .Set("user_password", user.user_password);
            Debug.WriteLine("Update " + update);
            dbClient.GetDatabase("Database").GetCollection<User>("User").UpdateOne(filter, update);
            //return new JsonResult("Updated Successfully");
            return new JsonResult(user);
        }
        


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<User>.Filter.Eq("user_nr", id);
            dbClient.GetDatabase("Database").GetCollection<User>("User").DeleteOne(filter);
            return new JsonResult("Kunde wurde erfolgreich gelöscht");
        }

    }
}

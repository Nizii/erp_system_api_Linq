using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Diagnostics;
using MongoDB.Bson;
using BCrypt.Net;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public UserController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
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
        public string[] Get(string user_name, string user_password)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string[] result_array = new string[2];
            
            foreach (var result in dbList)
            {
                if(result.user_name.Equals(user_name)) {
                    if(BCrypt.Net.BCrypt.Verify(user_password, result.user_password)) {
                        result_array[0] = result.user_nr.ToString();
                        result_array[1] = result.user_name;
                        break;
                    }
                    result_array[0] = "Password incorrect";
                    break;
                } 
                else
                {
                    result_array[0] = "User Not found";
                }
            }
            return result_array;
        }


        [HttpPost]
        public string[] Post(User user_object)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string[] result_array = new string[2];
            bool name_is_not_double = true;
            
            foreach (var user_name in dbList)
            {
                if (user_name.Equals(user_object.user_name))
                {
                    result_array[0] = "Name bereits vergeben";
                    name_is_not_double = false; 
                    break;
                }
            }

            if (name_is_not_double)
            {
                int salt = 12;
                user_object.user_password = BCrypt.Net.BCrypt.HashPassword(user_object.user_password, salt);

                int lastUserId = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable().Count();
                user_object.user_nr = lastUserId + 1;
                dbClient.GetDatabase("Database").GetCollection<User>("User").InsertOne(user_object);
                result_array[0] = user_object.user_nr.ToString();
                result_array[1] = user_object.user_name;
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

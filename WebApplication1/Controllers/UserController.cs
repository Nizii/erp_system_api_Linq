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
        public string[] Get(string user_name)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string[] result_array = new string[3];

            foreach (var result in dbList)
            {
                if(result.user_name.Equals(user_name)) {
                    result_array[0] = "User found";
                    result_array[1] = result.user_nr.ToString();
                    result_array[2] = result.user_password;
                    break;
                } 
                else
                {
                    result_array[0] = "User Not found " + user_name;
                }
            }
            return result_array;
        }


        [HttpPost]
        public string Post(User user_object)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable();
            string result = "";
            bool name_is_not_double = true;
            
            foreach (var user_name in dbList)
            {
                if (user_name.Equals(user_object.user_name))
                {
                    result = "Name bereits vergeben";
                    name_is_not_double = false; 
                    break;
                }
            }

            if (name_is_not_double)
            {
                int lastUserId = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable().Count();
                user_object.user_nr = lastUserId + 1;
                dbClient.GetDatabase("Database").GetCollection<User>("User").InsertOne(user_object);
                result = "Erfolgreich Registriert";
            }

            return result;
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

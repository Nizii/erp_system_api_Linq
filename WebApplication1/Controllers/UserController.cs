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
            string[] resultArray = new string[3];

            foreach (var result in dbList)
            {
                if(result.user_name.Equals(user_name)) {
                    resultArray[0] = "User found";
                    resultArray[1] = result.user_nr.ToString();
                    resultArray[2] = result.user_password;
                    break;
                } 
                else
                {
                    resultArray[0] = "User Not found " + user_name;
                }
            }
            return resultArray;
        }


        [HttpPost]
        public JsonResult Post(User user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            int lastUserId = dbClient.GetDatabase("Database").GetCollection<User>("User").AsQueryable().Count();
            user.user_nr = lastUserId + 1;
            dbClient.GetDatabase("Database").GetCollection<User>("User").InsertOne(user);
            return new JsonResult("Added Successfully");
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

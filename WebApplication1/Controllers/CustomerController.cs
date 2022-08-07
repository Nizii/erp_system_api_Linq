using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").AsQueryable();
            return new JsonResult(dbList);
        }


        [HttpPost]
        public JsonResult Post(Customer cus)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            int LastCustomerId = dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").AsQueryable().Count();
            cus.customer_nr = LastCustomerId + 1;
            dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").InsertOne(cus);
            return new JsonResult("Added Successfully");
        }

        
        [HttpPut]
        public JsonResult Put(Customer cus)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<Customer>.Filter.Eq("customer_nr", cus.customer_nr);
            var update = Builders<Customer>.Update.Set("surname", cus.surname)
                                                    .Set("lastname", cus.lastname)
                                                    .Set("dob", cus.dob)
                                                    .Set("street", cus.street)
                                                    .Set("postcode", cus.postcode)
                                                    .Set("country", cus.country)
                                                    .Set("cellphone", cus.cellphone)
                                                    .Set("landlinephone", cus.landlinephone)
                                                    .Set("note", cus.note)
                                                    .Set("email", cus.email);
            dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").UpdateOne(filter, update);
            return new JsonResult("Updated Successfully");
        }
        


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<Customer>.Filter.Eq("customer_nr", id);
            dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").DeleteOne(filter);
            return new JsonResult("Kunde wurde erfolgreich gelöscht");
        }

        /*
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }


*/
    }
}

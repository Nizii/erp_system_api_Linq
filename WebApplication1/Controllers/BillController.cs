using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            var dbList = dbClient.GetDatabase("Database").GetCollection<Bill>("Bill").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Bill bill)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            int LastDepartmentId = dbClient.GetDatabase("Database").GetCollection<Bill>("Bill").AsQueryable().Count();
            bill.bill_nr = LastDepartmentId + 1;

            dbClient.GetDatabase("Database").GetCollection<Bill>("Bill").InsertOne(bill);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Bill bill)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            var filter = Builders<Bill>.Filter.Eq("bill_nr", bill.bill_nr);

            var update = Builders<Bill>.Update.Set("DepartmentName", bill.customer_name);



            dbClient.GetDatabase("Database").GetCollection<Bill>("Bill").UpdateOne(filter,update);

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            var filter = Builders<Bill>.Filter.Eq("bill_nr", id);


            dbClient.GetDatabase("Database").GetCollection<Bill>("Bill").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }



    }
}

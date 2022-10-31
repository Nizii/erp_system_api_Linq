using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using WebApplication1.Models;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public ProductController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<Product>("Product").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<Product>("Product").AsQueryable();
            Product product = null;
            foreach (var product_from_db in dbList)
            {
                if (id.Equals(product_from_db.product_nr))
                {
                    product = product_from_db;
                    break;
                }
            }
            return new JsonResult(product);
        }


        [HttpPost]
        public JsonResult Post(Product pro)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            int LastProductId = dbClient.GetDatabase("Database").GetCollection<Product>("Product").AsQueryable().Count();
            pro.product_nr = LastProductId + 1;
            dbClient.GetDatabase("Database").GetCollection<Product>("Product").InsertOne(pro);
            return new JsonResult("Added Successfully");
        }

       
        [HttpPut]
        public JsonResult Put(Product pro)
        {
 
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<Product>.Filter.Eq("product_nr", pro.product_nr);
            Debug.WriteLine("Filter "+filter);
            /*
            Debug.WriteLine("Customer_nr " + cus.customer_nr);
            Debug.WriteLine("Debug " + cus.surname);
            Debug.WriteLine("Debug " + cus.lastname);
            Debug.WriteLine("Debug " + cus.dob);
            Debug.WriteLine("Debug " + cus.street);
            Debug.WriteLine("Debug " + cus.postcode);
            Debug.WriteLine("Debug " + cus.country);
            Debug.WriteLine("Debug " + cus.cellphone);
            Debug.WriteLine("Debug " + cus.landlinephone);
            Debug.WriteLine("Debug " + cus.note);
            Debug.WriteLine("Debug " + cus.email);
            */
            var update = Builders<Product>.Update.Set("product_name", pro.product_name)
                                                    .Set("product_size", pro.product_size)
                                                    .Set("description", pro.description)
                                                    .Set("units_available", pro.units_available)
                                                    .Set("unit", pro.unit)
                                                    .Set("purchasing_price_per_unit", pro.purchasing_price_per_unit)
                                                    .Set("selling_price_per_unit", pro.selling_price_per_unit);
            Debug.WriteLine("Update " + update);
            dbClient.GetDatabase("Database").GetCollection<Product>("Product").UpdateOne(filter, update);
            //return new JsonResult("Updated Successfully");
            return new JsonResult(pro);
        }
        


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<Product>.Filter.Eq("product_nr", id);
            dbClient.GetDatabase("Database").GetCollection<Product>("Product").DeleteOne(filter);
            return new JsonResult("Produkt wurde erfolgreich gelöscht");
        }
    }
}

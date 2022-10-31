using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBillController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CustomerBillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            var dbList = dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").AsQueryable();
            CustomerBill bill = null;
            foreach (var bill_from_db in dbList)
            {
                if (id.Equals(bill_from_db.customer_bill_nr))
                {
                    bill = bill_from_db;
                    break;
                }
            }
            return new JsonResult(bill);
        }

        [HttpPost]
        public JsonResult Post(CustomerBill bill)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            int temp_customer_bill_nr = dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").AsQueryable().Count();
            bill.customer_bill_nr = temp_customer_bill_nr + 1;

            dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").InsertOne(bill);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(CustomerBill bill)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<CustomerBill>.Filter.Eq("customer_bill_nr", bill.customer_bill_nr);

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
            
            var update = Builders<Product>.Update.Set("product_name", pro.product_name)
                                                    .Set("product_size", pro.product_size)
                                                    .Set("description", pro.description)
                                                    .Set("units_available", pro.units_available)
                                                    .Set("unit", pro.unit)
                                                    .Set("purchasing_price_per_unit", pro.purchasing_price_per_unit)
                                                    .Set("selling_price_per_unit", pro.selling_price_per_unit);
            dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").UpdateOne(filter, update);
            */
            //return new JsonResult("Updated Successfully");
            return new JsonResult(bill);
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));

            var filter = Builders<CustomerBill>.Filter.Eq("customer_bill_nr", id);


            dbClient.GetDatabase("Database").GetCollection<CustomerBill>("CustomerBill").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }



    }
}

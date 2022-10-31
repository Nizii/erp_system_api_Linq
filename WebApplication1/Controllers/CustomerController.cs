using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache) : base(configuration, env, cache)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            CheckAuthentication();
            List<Customer> customerList = new List<Customer>();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer", con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < count; i++)
                    {
                        Customer customer = new Customer
                        {
                            customer_nr = (int)reader["customer_nr"],
                            surname = reader["surname"].ToString(),
                            lastname = reader["lastname"].ToString(),
                            dob = reader["dob"].ToString(),
                            street = reader["street"].ToString(),
                            nr = reader["nr"].ToString(),
                            postcode = reader["postcode"].ToString(),
                            country = reader["country"].ToString(),
                            cellphone = reader["cellphone"].ToString(),
                            landlinephone = reader["landlinephone"].ToString(),
                            note = reader["note"].ToString(),
                            email = reader["email"].ToString()
                        };
                        customerList.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var dbList = dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").AsQueryable();
            Customer customer = null;
            foreach (var customer_from_db in dbList)
            {
                if (id.Equals(customer_from_db.customer_nr))
                {
                    customer = customer_from_db;
                    break;
                }
            }
            return new JsonResult(customer);
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
            Debug.WriteLine("Filter "+filter);

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

            var update = Builders<Customer>.Update.Set("surname", cus.surname)
                                                    .Set("lastname", cus.lastname)
                                                    .Set("dob", cus.dob)
                                                    .Set("street", cus.street)
                                                    .Set("nr", cus.nr)
                                                    .Set("postcode", cus.postcode)
                                                    .Set("country", cus.country)
                                                    .Set("cellphone", cus.cellphone)
                                                    .Set("landlinephone", cus.landlinephone)
                                                    .Set("note", cus.note)
                                                    .Set("email", cus.email);
            Debug.WriteLine("Update " + update);
            dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").UpdateOne(filter, update);
            //return new JsonResult("Updated Successfully");
            return new JsonResult(cus);
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ConnectionStringForDatabase"));
            var filter = Builders<Customer>.Filter.Eq("customer_nr", id);
            dbClient.GetDatabase("Database").GetCollection<Customer>("Customer").DeleteOne(filter);
            return new JsonResult("Kunde wurde erfolgreich gelöscht");
        }
    }
}

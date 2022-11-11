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
    public class CustomerController : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
            string connStr = ConfigurationExtensions.GetConnectionString(_configuration, "ConnectionStringForDatabase");
            con = new MySqlConnection(connStr);
        }


        [HttpGet]
        public JsonResult Get()
        {
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
 
            Customer customer = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer where customer_nr = "+id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customer = new Customer
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
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customer);
        }

        [HttpPost]
        public JsonResult Post(Customer cus)
        {

            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO customer (surname,lastname,dob,street,nr,postcode,country,cellphone,landlinephone,note,email) VALUES (@surname,@lastname,@dob,@street,@nr,@postcode,@country,@cellphone,@landlinephone,@note,@email)", con);
                cmd.Parameters.AddWithValue("@surname", cus.surname);
                cmd.Parameters.AddWithValue("@lastname", cus.lastname);
                cmd.Parameters.AddWithValue("@dob", "2022-02-02");
                cmd.Parameters.AddWithValue("@street", cus.street);
                cmd.Parameters.AddWithValue("@nr", cus.nr);
                cmd.Parameters.AddWithValue("@postcode", cus.postcode);
                cmd.Parameters.AddWithValue("@country", cus.country);
                cmd.Parameters.AddWithValue("@cellphone", cus.cellphone);
                cmd.Parameters.AddWithValue("@landlinephone", cus.landlinephone);
                cmd.Parameters.AddWithValue("@note", cus.note);
                cmd.Parameters.AddWithValue("@email", cus.email);
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }

        [HttpPut]
        public JsonResult Put(Customer cus)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("Update customer set surname=@surname, lastname=@lastname, dob=@dob, street=@street, nr=@nr, postcode=@postcode, country=@country, cellphone=@cellphone, landlinephone=@landlinephone, note=@note, email=@email where customer_nr ="+cus.customer_nr, con);
                cmd.Parameters.AddWithValue("@surname", cus.surname);
                cmd.Parameters.AddWithValue("@lastname", cus.lastname);
                cmd.Parameters.AddWithValue("@dob", "2022-02-02");
                cmd.Parameters.AddWithValue("@street", cus.street);
                cmd.Parameters.AddWithValue("@nr", cus.nr);
                cmd.Parameters.AddWithValue("@postcode", cus.postcode);
                cmd.Parameters.AddWithValue("@country", cus.country);
                cmd.Parameters.AddWithValue("@cellphone", cus.cellphone);
                cmd.Parameters.AddWithValue("@landlinephone", cus.landlinephone);
                cmd.Parameters.AddWithValue("@note", cus.note);
                cmd.Parameters.AddWithValue("@email", cus.email);
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("DELETE FROM customer WHERE customer_nr ="+id, con);
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult("Done");
        }
    }
}

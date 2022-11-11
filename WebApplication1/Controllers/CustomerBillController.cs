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
    public class CustomerBillController : ControllerBase
    {
        protected MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CustomerBillController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<CustomerBill> customerBillList = new List<CustomerBill>();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer_bill", con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < count; i++)
                    {
                        CustomerBill customerBill = new CustomerBill
                        {
                            customer_bill_nr = (int)reader["customer_bill_nr"],
                            company_name = reader["company_name"].ToString(),
                            contact_person = reader["contact_person"].ToString(),
                            customer_street = reader["customer_street"].ToString(),
                            amount = reader["amount"].ToString(),
                            currency = reader["currency"].ToString(),
                            issued_on = reader["issued_on"].ToString(),
                            payment_date = reader["payment_date"].ToString(),
                        };
                        customerBillList.Add(customerBill);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerBillList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            CustomerBill customerBill = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from customer_bill where customer_bill_nr = " + id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customerBill = new CustomerBill
                    {
                        customer_bill_nr = (int)reader["customer_bill_nr"],
                        company_name = reader["company_name"].ToString(),
                        contact_person = reader["contact_person"].ToString(),
                        customer_street = reader["customer_street"].ToString(),
                        amount = reader["amount"].ToString(),
                        currency = reader["currency"].ToString(),
                        issued_on = reader["issued_on"].ToString(),
                        payment_date = reader["payment_date"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(customerBill);
        }

        [HttpPost]
        public JsonResult Post(CustomerBill bill)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO customer_bill (company_name,contact_person,customer_street,amount,currency,issued_on,payment_date) VALUES (@company_name,@contact_person,@customer_street,@amount,@currency,@issued_on,@payment_date)", con);
                cmd.Parameters.AddWithValue("@company_name", bill.company_name);
                cmd.Parameters.AddWithValue("@contact_person", bill.contact_person);
                cmd.Parameters.AddWithValue("@customer_street", bill.customer_street);
                cmd.Parameters.AddWithValue("@amount", bill.amount);
                cmd.Parameters.AddWithValue("@currency", bill.currency);
                cmd.Parameters.AddWithValue("@issued_on", "9999-99-99");
                cmd.Parameters.AddWithValue("@payment_date", "9999-99-99");
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
        public JsonResult Put(CustomerBill bill)
        {
            try
            {
                con.Open();
                var cmd = new MySqlCommand("Update customer_bill set company_name=@company_name, contact_person=@contact_person, customer_street=@customer_street, amount=@amount, currency=@currency, issued_on=@issued_on, payment_date=@payment_date where customer_nr =" + bill.customer_bill_nr, con);
                cmd.Parameters.AddWithValue("@company_name", bill.company_name);
                cmd.Parameters.AddWithValue("@contact_person", bill.contact_person);
                cmd.Parameters.AddWithValue("@customer_street", bill.customer_street);
                cmd.Parameters.AddWithValue("@amount", bill.amount);
                cmd.Parameters.AddWithValue("@currency", bill.currency);
                cmd.Parameters.AddWithValue("@issued_on", "9999-99-99");
                cmd.Parameters.AddWithValue("@payment_date", "9999-99-99");
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
                var cmd = new MySqlCommand("DELETE FROM customer_bill WHERE customer_bill_nr =" + id, con);
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

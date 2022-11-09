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
    public class ProductController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public ProductController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache) : base(configuration, env, cache)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            CheckAuthentication();
            List<Product> productList = new List<Product>();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from product", con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < count; i++)
                    {
                        Product product = new Product
                        {
                            product_nr = (int) reader["product_nr"],
                            product_name = reader["product_name"].ToString(),
                            product_size = reader["product_size"].ToString(),
                            description = reader["description"].ToString(),
                            units_available = (int) reader["units_available"],
                            unit = reader["unit"].ToString(),
                            purchasing_price_per_unit = (double) reader["purchasing_price_per_unit"],
                            selling_price_per_unit = (double) reader["selling_price_per_unit"],
                        };
                        productList.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(productList);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            CheckAuthentication();
            Product product = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * from product where product_nr = " + id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                        product = new Product
                        {
                            product_nr = (int)reader["product_nr"],
                            product_name = reader["product_name"].ToString(),
                            product_size = reader["product_size"].ToString(),
                            description = reader["description"].ToString(),
                            units_available = (int)reader["units_available"],
                            unit = reader["unit"].ToString(),
                            purchasing_price_per_unit = (double)reader["purchasing_price_per_unit"],
                            selling_price_per_unit = (double)reader["selling_price_per_unit"],
                        };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return new JsonResult(product);
        }


        [HttpPost]
        public JsonResult Post(Product pro)
        {
            CheckAuthentication();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("INSERT INTO product (product_name,product_size,description,units_available,unit,purchasing_price_per_unit,selling_price_per_unit) VALUES (@product_name,@product_size,@description,@unit,@currency,@purchasing_price_per_unit,@selling_price_per_unit)", con);
                cmd.Parameters.AddWithValue("@product_name", pro.product_name);
                cmd.Parameters.AddWithValue("@product_size", pro.product_size);
                cmd.Parameters.AddWithValue("@description", pro.description);
                cmd.Parameters.AddWithValue("@units_available", pro.units_available);
                cmd.Parameters.AddWithValue("@unit", pro.unit);
                cmd.Parameters.AddWithValue("@purchasing_price_per_unit", pro.purchasing_price_per_unit);
                cmd.Parameters.AddWithValue("@selling_price_per_unit", pro.selling_price_per_unit);
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
        public JsonResult Put(Product pro)
        {
            CheckAuthentication();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("Update product set product_name=@product_name, product_size=@product_size, description=@description, units_available=@units_available, unit=@unit, purchasing_price_per_unit=@purchasing_price_per_unit, selling_price_per_unit=@selling_price_per_unit where product_nr =" + pro.product_nr, con);
                cmd.Parameters.AddWithValue("@product_name", pro.product_name);
                cmd.Parameters.AddWithValue("@product_size", pro.product_size);
                cmd.Parameters.AddWithValue("@description", pro.description);
                cmd.Parameters.AddWithValue("@units_available", pro.units_available);
                cmd.Parameters.AddWithValue("@unit", pro.unit);
                cmd.Parameters.AddWithValue("@purchasing_price_per_unit", pro.purchasing_price_per_unit);
                cmd.Parameters.AddWithValue("@selling_price_per_unit", pro.selling_price_per_unit);
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
            CheckAuthentication();
            try
            {
                con.Open();
                var cmd = new MySqlCommand("DELETE FROM product WHERE product_nr =" + id, con);
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

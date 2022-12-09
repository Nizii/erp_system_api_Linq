using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using ErpSystemDbContext;
using Customer = ErpSystemDbContext.Customer;
using WebApplication1;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            /*
            if (HttpContext.Session.Get<User>("TestUser") is null)
                return new JsonResult("Not same session id");
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var query = from it in model.Customers orderby it.CustomerNr select it;
          
            List<Customer> list = new List<Customer>();
            foreach (Customer cus in query)
                list.Add(cus);
            return new JsonResult(list);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            /*
            if (HttpContext.Session.Get<User>("TestUser") is null)
                return new JsonResult("Not same session id");
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            return new JsonResult(from it in model.Customers where (it.CustomerNr == id) select it);
        }
        
        [HttpPost]
        public JsonResult Post(Customer cus)
        {
            /*
            if (HttpContext.Session.Get<User>("TestUser") is null)
                return new JsonResult("Not same session id");
            */
            try
            {
                Customer order = new Customer
                {
                    CompanyName = cus.CompanyName,
                    Surname = cus.Surname,
                    Lastname = cus.Lastname,
                    Dob = cus.Dob,
                    Street = cus.Street,
                    Nr = cus.Nr,
                    Postcode = cus.Postcode,
                    Country = cus.Country,
                    Cellphone = cus.Cellphone,
                    Landlinephone = cus.Landlinephone,
                    Note = cus.Note,
                    Email = cus.Email,
                };

                ErpSystemDbDataContext model = new ErpSystemDbDataContext();
                model.Customers.InsertOnSubmit(order);
                model.SubmitChanges();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert failed with Ex: " + ex);
            }
            return new JsonResult("Done");
        }

        [HttpPut]
        public JsonResult Put(Customer cus)
        {
            /*
            if (HttpContext.Session.Get<User>("TestUser") is null)
                return new JsonResult("Not same session id");
            */
            return new JsonResult("Done");
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            /*
            if (HttpContext.Session.Get<User>("TestUser") is null)
                return new JsonResult("Not same session id");
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var deleteOrderDetails = from it in model.Customers where it.CustomerNr == id select it;
            foreach (var row in deleteOrderDetails)
            {
                model.Customers.DeleteOnSubmit(row);
            }
            model.SubmitChanges();
            return new JsonResult("Done");
        }
    }
}

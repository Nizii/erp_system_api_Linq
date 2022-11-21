using Microsoft.AspNetCore.Hosting;
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

namespace WebApplication1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public CustomerController(){}

        [HttpGet]
        public JsonResult Get()
        {
            // Session prüfen
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            
            // Linq Query request
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var query = from it in model.Customers orderby it.CustomerNr select it;
          
            //  Liste füllen
            List<Customer> list = new List<Customer>();
            foreach (Customer cus in query)
                list.Add(cus);
            
            return new JsonResult(list);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            // Session prüfen
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);
           
            // Linq Query request
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            return new JsonResult(from it in model.Customers where (it.CustomerNr == id) select it);
        }
        
        [HttpPost]
        public JsonResult Post(Customer cus)
        {
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);

            return new JsonResult("Done");
        }

        [HttpPut]
        public JsonResult Put(Customer cus)
        {
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);

            return new JsonResult("Done");
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);

            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var deleteOrderDetails = from it in model.Customers where it.CustomerNr == id select it;
            foreach (var row in deleteOrderDetails)
            {
                model.Customers.DeleteOnSubmit(row);
            }
            return new JsonResult("Done");
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using ErpSystemDbContext;
using Product = ErpSystemDbContext.Product;
using WebApplication1;
using System.Diagnostics;
using System;

namespace WebApplication1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    { 
        [HttpGet]
        public JsonResult Get()
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
            {
                return new JsonResult(null);
            }
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var query = from it in model.Products orderby it.ProductNr select it;

            List<Product> list = new List<Product>();
            foreach (Product cus in query)
                list.Add(cus);
            return new JsonResult(list);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            return new JsonResult(from it in model.Products where (it.ProductNr == id) select it);
        }

        [HttpPost]
        public JsonResult Post(Product pro)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);
            */
            try
            {
                ErpSystemDbDataContext model = new ErpSystemDbDataContext();
                model.Products.InsertOnSubmit(pro);
                model.SubmitChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Insert failed with Ex: " + ex);
            }
            return new JsonResult("Done");
        }

        [HttpPut]
        public JsonResult Put(Product pro)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);
            */
            return new JsonResult("Done");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            /*
            if (HttpContext.Session.Get("Nizam") is null)
                return new JsonResult(null);
            */
            ErpSystemDbDataContext model = new ErpSystemDbDataContext();
            var deleteOrderDetails = from it in model.Products where it.ProductNr == id select it;
            foreach (var row in deleteOrderDetails)
            {
                model.Products.DeleteOnSubmit(row);
            }
            model.SubmitChanges();
            return new JsonResult("Done");
        }
    }
}

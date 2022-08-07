using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Bill
    {
        public ObjectId Id { get; set; }

        public string customer_name { get; set; }

        public string customer_street { get; set; }

        public string customer_postcode { get; set; }

        public Int32 amount { get; set; }

        public string currency { get; set; }

        public DateTime issued_on { get; set; }

        public DateTime deadline { get; set; }

        public Int32  bill_nr { get; set; }
    }
}

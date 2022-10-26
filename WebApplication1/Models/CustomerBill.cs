using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CustomerBill
    {
        public ObjectId Id { get; set; }

        public String company_name { get; set; }
        public String customer_name { get; set; }

        public String customer_street { get; set; }

        public String customer_postcode { get; set; }

        public Double amount { get; set; }

        public String currency { get; set; }

        public String issued_on { get; set; }

        public String deadline { get; set; }

        public Int32 customer_bill_nr { get; set; }
    }
}

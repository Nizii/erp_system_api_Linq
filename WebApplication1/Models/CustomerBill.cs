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
        public String contact_person { get; set; }

        public String customer_street { get; set; }

        public String customer_postcode { get; set; }

        public String amount { get; set; }

        public String currency { get; set; }

        public String issued_on { get; set; }

        public String payment_date { get; set; }

        public Int32 customer_bill_nr { get; set; }
    }
}

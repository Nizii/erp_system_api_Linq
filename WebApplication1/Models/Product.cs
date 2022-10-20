using MongoDB.Bson;
using System;

namespace WebApplication1.Models
{
    public class Product
    {
        public ObjectId Id { get; set; }

        public Int32 product_nr { get; set; }

        public string product_name { get; set; }

        public Int32 product_size { get; set; }

        public string description { get; set; }

        public string units_available { get; set; }

        public string unit { get; set; }

        public Int32 purchasing_price_per_unit { get; set; }

        public Int32 selling_price_per_unit { get; set; }
    }
}

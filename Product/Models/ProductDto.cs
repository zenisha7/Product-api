using System;


namespace Product.Models
{
    public class ProductDto
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public int StockLvl { get; set; }
        public double ResellPrice { get; set; }
    }
}

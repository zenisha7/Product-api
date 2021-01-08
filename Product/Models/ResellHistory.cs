using System;
namespace Product.Models
{
    public class ResellHistory
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public double  ResellPrice{ get; set; }
        public DateTime DateTime { get; set; }
    }
}

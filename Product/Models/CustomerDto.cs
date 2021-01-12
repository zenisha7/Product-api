using System;

namespace Product.Models
{
    public class CustomerDto
    {
        public Guid CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string MobNumber { get; set; }
        public bool PurchaseAbility { get; set;  }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public class MockCustomerRepository
    {
        public static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer {CustomerID = new Guid("1554a8d9-f114-2hd4-8g33-6205325255j4"),
                FirstName = "Customer",
                LastName = "AAAAA",
                Email = "CustomerAAA@hotmail.com",
                Address = "Address AA",
                Postcode = "NG6",
                DateOfBirth = new DateTime(1987, 10, 12),
                MobNumber = "1111111",
                PurchaseAbility = true
            },
            new Customer {CustomerID = new Guid("1354a8d9-g114-2gd4-8g33-4535325265s0"),
                FirstName = "Customer",
                LastName = "BBBBB",
                Email = "CustomerBB@hotmail.com",
                Address = "Address BB",
                Postcode = "NG7",
                DateOfBirth= new DateTime(1993, 12, 20),
                MobNumber = "2222222",
                PurchaseAbility = true
                
                
            },
            new Customer {CustomerID = new Guid("4dg6834g-6769-03g2-jc6x-e4d6e2ac8909"),
                FirstName = "Customer",
                LastName = "CCCCCC",
                Email = "CustomerCC@hotmail.com",
                Address = "Address CC",
                Postcode = "NG5",
                DateOfBirth = new DateTime(1994, 10, 12),
                MobNumber = "3333333",
                PurchaseAbility = true
                
            }
        };

        public static Customer GetCustomerByID(Guid id)
        {
            var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();

            return getCustomer;
        }
    }
}

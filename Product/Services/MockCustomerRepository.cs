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
            new Customer {CustomerID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                FirstName = "Customer",
                LastName = "AAAAA",
                Email = "CustomerAAA@hotmail.com",
                Address = "Address AA",
                Postcode = "NG6",
                MobNumber = "1111111",
                PurchaseAbility = true
            },
            new Customer {CustomerID = new Guid("36004789-ebe9-47f9-a45c-6502232e5304"),
                FirstName = "Customer",
                LastName = "BBBBB",
                Email = "CustomerBB@hotmail.com",
                Address = "Address BB",
                Postcode = "NG7",
                MobNumber = "2222222",
                PurchaseAbility = true
                
                
            },
            new Customer {CustomerID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                FirstName = "Customer",
                LastName = "CCCCCC",
                Email = "CustomerCC@hotmail.com",
                Address = "Address CC",
                Postcode = "NG5",
                MobNumber = "3333333",
                PurchaseAbility = true
                
            }
        };

        public static Customer GetCustomerByID(Guid id)
        {
            var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();

            return getCustomer;
        }

        public static Customer SetPurchaseProductAbility(Guid id, bool purchaseAbility)
        {
            var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();
            if (getCustomer != null)
            {
                getCustomer.PurchaseAbility = purchaseAbility;
            }
            return getCustomer;
        }

    }
}

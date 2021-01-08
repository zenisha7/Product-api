using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomers();
        Task<Customer> GetCustomerByID(Guid id);
        Task<Customer> SetPurchaseProductAbility(Guid id, bool purchase);
    }
}

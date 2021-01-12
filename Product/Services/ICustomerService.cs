using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetCustomers();
        Task<CustomerDto> GetCustomerByID(Guid id);
        Task<CustomerDto> SetPurchaseProductAbility(Guid id, bool purchase);
    }
}

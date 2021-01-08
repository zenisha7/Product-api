using Microsoft.Extensions.Logging;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly List<Customer> _customers;

        public CustomerRepository(ILogger<CustomerRepository> logger)
        {
            _customers = MockCustomerRepository._customers;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                var getCustomer = _customers;
                _logger.LogInformation($"Returning total of {getCustomer.Count} cutomers.");
                return await Task.FromResult(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }
        public async Task<Customer> GetCustomerByID(Guid id)
        {
            try
            {
                var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();
                _logger.LogInformation($"Returning customer with their id {id}");
                return await Task.FromResult(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<Customer> SetPurchaseProductAbility(Guid id, bool purchase)
        {
            try
            {
                var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();
                if(getCustomer != null)
                {
                    getCustomer.PurchaseAbility = purchase;
                }
                _logger.LogInformation($"Returning customer with their id {id} and their purchase ability {getCustomer.PurchaseAbility}");
                return await Task.FromResult(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }
    }
}

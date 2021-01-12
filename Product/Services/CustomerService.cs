using Microsoft.Extensions.Logging;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly List<CustomerDto> _customers;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _customers = FakeCustomerService._customers;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> GetCustomers()
        {
            try
            {
                var getCustomer = _customers;
                _logger.LogInformation($"Returning total of {getCustomer.Count} cutomers.");
                return await Task.FromResult(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }
        public async Task<CustomerDto> GetCustomerByID(Guid id)
        {
            try
            {
                var getCustomer = _customers.Where(g => g.CustomerID == id).FirstOrDefault();
                _logger.LogInformation($"Returning customer with their id {id}");
                return await Task.FromResult(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }

        public async Task<CustomerDto> SetPurchaseProductAbility(Guid id, bool purchase)
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
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }
    }
}

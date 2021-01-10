using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.Models;
using Product.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Tests
{
    public class CustomerRepositoryTests
    {
        private ICustomerRepository _customerRepo;
        private Mock<ILogger<CustomerRepository>> _logger;


        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CustomerRepository>>();
            _customerRepo = new CustomerRepository(_logger.Object);

        }

        #region GetCustomers
        [Test]
        public async Task GetCustomers()
        {
            var expectedResult = MockCustomerRepository._customers;
            var actualResult = await _customerRepo.GetCustomers();
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<Customer>>(actualResult);

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].CustomerID, actualResult[i].CustomerID);
                Assert.AreEqual(expectedResult[i].FirstName, actualResult[i].FirstName);
                Assert.AreEqual(expectedResult[i].LastName, actualResult[i].LastName);
                Assert.AreEqual(expectedResult[i].Address, actualResult[i].Address);
                Assert.AreEqual(expectedResult[i].Postcode, actualResult[i].Postcode);
                Assert.AreEqual(expectedResult[i].MobNumber, actualResult[i].MobNumber);
                Assert.AreEqual(expectedResult[i].Email, actualResult[i].Email);
                Assert.AreEqual(expectedResult[i].PurchaseAbility, actualResult[i].PurchaseAbility);
            }
        }
        #endregion GetCustomers

        #region GetCustomerByID
        [Test]
        public async Task GetCustomerWithID()
        {
            var expectedResult = MockCustomerRepository._customers[1];
            var actualResult = await _customerRepo.GetCustomerByID(expectedResult.CustomerID);

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Customer>(actualResult);
            Assert.AreEqual(expectedResult.CustomerID, actualResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, actualResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, actualResult.LastName);
            Assert.AreEqual(expectedResult.Address, actualResult.Address);
            Assert.AreEqual(expectedResult.Postcode, actualResult.Postcode);
            Assert.AreEqual(expectedResult.Email, actualResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, actualResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, actualResult.PurchaseAbility);
        }
        #endregion GetCustomerByID
        
        #region SetPurchaseProductAbility
        [Test]
        public async Task SetPurchaseProductAbility()
        {
            var expectedResult = MockCustomerRepository._customers[1];
            var actualResult = await _customerRepo.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility);

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Customer>(actualResult);
            Assert.AreEqual(expectedResult.CustomerID, actualResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, actualResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, actualResult.LastName);
            Assert.AreEqual(expectedResult.Address, actualResult.Address);
            Assert.AreEqual(expectedResult.Postcode, actualResult.Postcode);
            Assert.AreEqual(expectedResult.Email, actualResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, actualResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, actualResult.PurchaseAbility);
        }
        #endregion SetPurchaseProductAbility

    }
}
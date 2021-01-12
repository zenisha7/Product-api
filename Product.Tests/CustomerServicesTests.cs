using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.Models;
using Product.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Tests
{
    public class CustomerServicesTests
    {
        private ICustomerService _customerRepo;
        private Mock<ILogger<CustomerService>> _logger;


        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CustomerService>>();
            _customerRepo = new CustomerService(_logger.Object);

        }

        #region GetCustomersTest
        [Test]
        public async Task GetCustomersTest()
        {
            var expectedResult = FakeCustomerService._customers;
            var actualResult = await _customerRepo.GetCustomers();
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<CustomerDto>>(actualResult);

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
        #endregion GetCustomersTest

        #region GetCustomerByIDTest
        [Test]
        public async Task GetCustomerByIDTest()
        {
            var expectedResult = FakeCustomerService._customers[1];
            var actualResult = await _customerRepo.GetCustomerByID(expectedResult.CustomerID);

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<CustomerDto>(actualResult);
            Assert.AreEqual(expectedResult.CustomerID, actualResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, actualResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, actualResult.LastName);
            Assert.AreEqual(expectedResult.Address, actualResult.Address);
            Assert.AreEqual(expectedResult.Postcode, actualResult.Postcode);
            Assert.AreEqual(expectedResult.Email, actualResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, actualResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, actualResult.PurchaseAbility);
        }
        #endregion GetCustomerByIDTest

        #region SetPurchaseProductAbilityTest
        [Test]
        public async Task SetPurchaseProductAbilityTest()
        {
            var expectedResult = FakeCustomerService._customers[1];
            var actualResult = await _customerRepo.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility);

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<CustomerDto>(actualResult);
            Assert.AreEqual(expectedResult.CustomerID, actualResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, actualResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, actualResult.LastName);
            Assert.AreEqual(expectedResult.Address, actualResult.Address);
            Assert.AreEqual(expectedResult.Postcode, actualResult.Postcode);
            Assert.AreEqual(expectedResult.Email, actualResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, actualResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, actualResult.PurchaseAbility);
        }
        #endregion SetPurchaseProductAbilityTest

    }
}
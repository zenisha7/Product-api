using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.Models;
using Product.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Tests
{
    public class CustomerServicesTests
    {
        private ICustomerService _customer;
        private Mock<ILogger<CustomerService>> _logger;
        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CustomerService>>();
            _customer = new CustomerService(_logger.Object);
        }

        #region GetCustomersTest
        [Test]
        public async Task GetCustomersTest()
        {
            var expectedResult = FakeCustomerService._customers;
            var actualResult = await _customer.GetCustomers();
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
        //Valid test should return customers
        public async Task GetCustomerByID_ValidTest()
        {
            var expectedResult = FakeCustomerService._customers[1];
            var actualResult = await _customer.GetCustomerByID(expectedResult.CustomerID);
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
        [Test]
        public async Task GetCustomerByID_InvalidTest()
        {
            Guid invalidID = new Guid();
            var expectedResult = FakeCustomerService.GetCustomerByID(invalidID);
            var actualResult = await _customer.GetCustomerByID(invalidID);
            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion GetCustomerByIDTest

        #region SetPurchaseProductAbilityTest
        [Test]
        //Valid test should return customers
        public async Task SetPurchaseProductAbility_ValidTest()
        {
            
            var expectedResult = FakeCustomerService._customers[1];
            var actualResult = await _customer.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility);
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
        [Test]
        //invalid test should return null
        public async Task SetPurchaseProductAbility_InvalidTest()
        {
            Guid invalidID = new Guid();
            var expectedResult = FakeCustomerService.SetPurchaseProductAbility(invalidID, true);
            var actualResult = await _customer.SetPurchaseProductAbility(invalidID, true);
            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion SetPurchaseProductAbilityTest
    }
}
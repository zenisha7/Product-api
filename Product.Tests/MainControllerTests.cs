using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.Controllers;
using Product.Models;
using Product.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Product.Tests
{
    public class MainControllerTests
    {
        private int stockLvl;
        private ProductDto objStock;
        private CustomerDto objCustomer;
        private Mock<IProductService> _stockRepo;
        private Mock<ICustomerService> _customerRepo;
        private Mock<ILogger<MainController>> _logger;
        private MainController _mainController;
        private List<ProductDto> stocks;
        private List<CustomerDto> customers;
        private List<ResellPrice> _resellPrice;
        private List<ResellHistory> _resellHistory;

        [SetUp]
        public void Setup()
        {
            stockLvl = 20;
            objStock = new ProductDto();
            objCustomer = new CustomerDto();
            _stockRepo = new Mock<IProductService>();
            _customerRepo = new Mock<ICustomerService>();
            _logger = new Mock<ILogger<MainController>>();
            _mainController = new MainController( _customerRepo.Object, _stockRepo.Object, _logger.Object);
            stocks = FakeProductService._product;
            customers = FakeCustomerService._customers;
            _resellPrice = FakeProductService._resellPrice;
            _resellHistory = FakeProductService._resellHistory;
        }

        #region GetStockTest
        [Test]
        public async Task GetStockTest()
        {
            _stockRepo.Setup(g => g.GetAllProducts()).ReturnsAsync(stocks).Verifiable();
            var actualResult = await _mainController.GetAllProducts();
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as IEnumerable<ProductDto>;
            var okStockResultList = okStockResult.ToList();

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(stocks.Count, okStockResultList.Count);
            for(int i = 0; i<stocks.Count; i++)
            {
                Assert.AreEqual(stocks[i].ID, okStockResultList[i].ID);
                Assert.AreEqual(stocks[i].ProductID, okStockResultList[i].ProductID);
                Assert.AreEqual(stocks[i].StockLvl, okStockResultList[i].StockLvl);
                Assert.AreEqual(stocks[i].ResellPrice, okStockResultList[i].ResellPrice);
            }
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.GetAllProducts(), Times.Once); //Times: num of times method expected to call
        }
        #endregion GetStockTest

        #region GetStockByProductIDTest
        [Test]
        public async Task GetStockByProductIDTest()
        {
            var expectedResult = stocks[1];
            _stockRepo.Setup(g => g.GetProductByProductID(expectedResult.ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetProductByProductID(expectedResult.ProductID);
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as ProductDto;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(expectedResult.ID, okStockResult.ID);
            Assert.AreEqual(expectedResult.ProductID, okStockResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, okStockResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, okStockResult.ResellPrice);
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.GetProductByProductID(expectedResult.ProductID), Times.Once); 
        }
        #endregion GetStockByProductIDTest

        #region GetStockByLvlTest
        [Test]
        public async Task GetStockByLvlTest()
        {
            _stockRepo.Setup(g => g.GetStockLvlOfProducts(stockLvl)).ReturnsAsync(stocks).Verifiable();
            var actualResult = await _mainController.GetStockLvlOfProducts(stockLvl);
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as IEnumerable<ProductDto>;
            var okStockResultList = okStockResult.ToList();

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(stocks.Count, okStockResultList.Count);
            for (int i = 0; i < stocks.Count; i++)
            {
                Assert.AreEqual(stocks[i].ID, okStockResultList[i].ID);
                Assert.AreEqual(stocks[i].ProductID, okStockResultList[i].ProductID);
                Assert.AreEqual(stocks[i].StockLvl, okStockResultList[i].StockLvl);
                Assert.AreEqual(stocks[i].ResellPrice, okStockResultList[i].ResellPrice);
            }
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.GetStockLvlOfProducts(stockLvl), Times.Once);
        }

        #endregion GetStockByLvlTest

        #region GetResellPriceTest
        [Test]
        public async Task GetResellPriceTest()
        {
            var expectedResult = _resellPrice[1];
            _stockRepo.Setup(g => g.GetResellPriceOfProducts(expectedResult.ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetResellPriceOfProducts(expectedResult.ProductID);
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as ResellPrice;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(expectedResult.ProductID, okStockResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrices, okStockResult.ResellPrices);
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.GetResellPriceOfProducts(expectedResult.ProductID), Times.Once);
        }
        #endregion GetResellPriceTest

        #region SetResellPriceTest
        [Test]
        public async Task SetResellPriceTest()
        {
            var expectedResult = stocks[1];
            _stockRepo.Setup(g => g.SetResellPriceofProducts(expectedResult.ID, expectedResult.ResellPrice)).ReturnsAsync(expectedResult).Verifiable();
            objStock.ProductID = expectedResult.ID;
            objStock.ResellPrice = expectedResult.ResellPrice;
            var actualResult = await _mainController.SetResellPriceOfProducts(objStock);
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as ProductDto;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(expectedResult.ID, okStockResult.ID);
            Assert.AreEqual(expectedResult.ProductID, okStockResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, okStockResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, okStockResult.ResellPrice);
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.SetResellPriceofProducts(expectedResult.ID, expectedResult.ResellPrice), Times.Once);
        }
        #endregion SetResellPriceTest

        #region SetStockLvlTest 
        [Test]
        public async Task SetStockLvlTest()
        {
            var expectedResult = stocks[1];
            _stockRepo.Setup(g => g.SetStockLvlOfProducts(expectedResult.ID, expectedResult.StockLvl)).ReturnsAsync(expectedResult).Verifiable();
            objStock.ProductID = expectedResult.ID;
            objStock.StockLvl = expectedResult.StockLvl;
            var actualResult = await _mainController.SetStockLvlOfProducts(objStock);
            var okResult = actualResult as OkObjectResult;
            var okStockResult = okResult.Value as ProductDto;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okStockResult);
            Assert.AreEqual(expectedResult.ID, okStockResult.ID);
            Assert.AreEqual(expectedResult.ProductID, okStockResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, okStockResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, okStockResult.ResellPrice);
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.SetStockLvlOfProducts(expectedResult.ID, expectedResult.StockLvl), Times.Once);
        }
        #endregion SetStockLvlTest

        #region GetResellHistoryTest
        [Test]
        public async Task GetResellHistoryTest()
        {
            var expectedResult = _resellHistory;
            _stockRepo.Setup(g => g.GetResellHistory(expectedResult[1].ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetResellHistory(expectedResult[1].ProductID);
            var okResult = actualResult as OkObjectResult;
            var okResellHistoryResult = okResult.Value as IEnumerable<ResellHistory>;
            var okResellHistoryResultList = okResellHistoryResult.ToList();

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okResellHistoryResult);
            Assert.AreEqual(stocks.Count, okResellHistoryResultList.Count);
            for (int i = 0; i < _resellHistory.Count; i++)
            {
                Assert.AreEqual(_resellHistory[i].ID, okResellHistoryResultList[i].ID);
                Assert.AreEqual(_resellHistory[i].ProductID, okResellHistoryResultList[i].ProductID);
                Assert.AreEqual(_resellHistory[i].ResellPrice, okResellHistoryResultList[i].ResellPrice);
                Assert.AreEqual(_resellHistory[i].DateTime, okResellHistoryResultList[i].DateTime);
            }
            _stockRepo.Verify();
            _stockRepo.Verify(g => g.GetResellHistory(expectedResult[1].ProductID), Times.Once);
        }
        #endregion GetResellHistoryTest

        #region GetCustomersTest
        [Test]
        public async Task GetCustomersTest()
        {
            _customerRepo.Setup(g => g.GetCustomers()).ReturnsAsync(customers).Verifiable();
            var actualResult = await _mainController.GetCustomers();
            var okResult = actualResult as OkObjectResult;
            var okCustomerResult = okResult.Value as IEnumerable<CustomerDto>;
            var okCustomerResultList = okCustomerResult.ToList();

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okCustomerResult);
            Assert.AreEqual(customers.Count, okCustomerResultList.Count);
            for (int i = 0; i < customers.Count; i++)
            {
                Assert.AreEqual(customers[i].CustomerID, okCustomerResultList[i].CustomerID);
                Assert.AreEqual(customers[i].FirstName, okCustomerResultList[i].FirstName);
                Assert.AreEqual(customers[i].LastName, okCustomerResultList[i].LastName);
                Assert.AreEqual(customers[i].Address, okCustomerResultList[i].Address);
                Assert.AreEqual(customers[i].Postcode, okCustomerResultList[i].Postcode);
                Assert.AreEqual(customers[i].Email, okCustomerResultList[i].Email);
                Assert.AreEqual(customers[i].MobNumber, okCustomerResultList[i].MobNumber);
                Assert.AreEqual(customers[i].PurchaseAbility, okCustomerResultList[i].PurchaseAbility);
            }
            _customerRepo.Verify();
            _customerRepo.Verify(g => g.GetCustomers(), Times.Once); //Times: num of times method expected to call
        }
        #endregion GetCustomersTest

        #region GetCustomerByIDTest
        [Test]
        public async Task GetCustomerByIDTest()
        {
            var expectedResult = customers[1];
            _customerRepo.Setup(g => g.GetCustomerByID(expectedResult.CustomerID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetCustomerByID(expectedResult.CustomerID);
            var okResult = actualResult as OkObjectResult;
            var okCustomerResult = okResult.Value as CustomerDto;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okCustomerResult);
         
            Assert.AreEqual(expectedResult.CustomerID, okCustomerResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, okCustomerResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, okCustomerResult.LastName);
            Assert.AreEqual(expectedResult.Address, okCustomerResult.Address);
            Assert.AreEqual(expectedResult.Postcode, okCustomerResult.Postcode);
            Assert.AreEqual(expectedResult.Email, okCustomerResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, okCustomerResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, okCustomerResult.PurchaseAbility);
           
            _customerRepo.Verify();
            _customerRepo.Verify(g => g.GetCustomerByID(expectedResult.CustomerID), Times.Once); //Times: num of times method expected to call
        }
        #endregion GetCustomerByIDTest

        #region SetPurchaseAbilityTest
        [Test]
        public async Task SetPurchaseAbilityTest()
        {
            var expectedResult = customers[1];
            _customerRepo.Setup(g => g.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility)).ReturnsAsync(expectedResult).Verifiable();
            objCustomer.CustomerID = expectedResult.CustomerID;
            objCustomer.PurchaseAbility = expectedResult.PurchaseAbility;
            var actualResult = await _mainController.SetPurchaseProductAbility(objCustomer);
            var okResult = actualResult as OkObjectResult;
            var okCustomerResult = okResult.Value as CustomerDto;

            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreNotEqual(okResult.StatusCode, 500);
            Assert.IsNotNull(okCustomerResult);
            Assert.AreEqual(expectedResult.CustomerID, okCustomerResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, okCustomerResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, okCustomerResult.LastName);
            Assert.AreEqual(expectedResult.Address, okCustomerResult.Address);
            Assert.AreEqual(expectedResult.Postcode, okCustomerResult.Postcode);
            Assert.AreEqual(expectedResult.Email, okCustomerResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, okCustomerResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, okCustomerResult.PurchaseAbility);

            _customerRepo.Verify();
            _customerRepo.Verify(g => g.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility), Times.Once); 
        }
        #endregion SetPurchaseAbilityTest
    }
}
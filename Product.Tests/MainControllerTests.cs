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
using System;

namespace Product.Tests
{
    public class MainControllerTests
    {
        private ProductDto objProduct;
        private CustomerDto objCustomer;
        private Mock<IProductService> _productService;
        private Mock<ICustomerService> _customerService;
        private Mock<ILogger<MainController>> _logger;
        private MainController _mainController;
        private List<ProductDto> products;
        private List<CustomerDto> customers;
        private List<ResellPrice> _resellPrice;
        private List<ResellHistory> _resellHistory;

        [SetUp]
        public void Setup()
        {
            objProduct = new ProductDto();
            objCustomer = new CustomerDto();
            _productService = new Mock<IProductService>();
            _customerService = new Mock<ICustomerService>();
            _logger = new Mock<ILogger<MainController>>();
            _mainController = new MainController( _customerService.Object, _productService.Object, _logger.Object);
            products = FakeProductService._product;
            customers = FakeCustomerService._customers;
            _resellPrice = FakeProductService._resellPrice;
            _resellHistory = FakeProductService._resellHistory;
        }

        #region GetAllProductTest
        [Test]
        public async Task GetAllProductTest()
        {
            _productService.Setup(g => g.GetAllProducts()).ReturnsAsync(products).Verifiable();
            var actualResult = await _mainController.GetAllProducts();
            var okResult = actualResult as OkObjectResult;
            var okProducts = okResult.Value as IEnumerable<ProductDto>;
            var okProductsList = okProducts.ToList();
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okProducts);
            Assert.AreEqual(products.Count, okProductsList.Count);
            for(int i = 0; i<products.Count; i++)
            {
                Assert.AreEqual(products[i].ID, okProductsList[i].ID);
                Assert.AreEqual(products[i].ProductID, okProductsList[i].ProductID);
                Assert.AreEqual(products[i].StockLvl, okProductsList[i].StockLvl);
                Assert.AreEqual(products[i].ResellPrice, okProductsList[i].ResellPrice);
            }
            _productService.Verify();
            _productService.Verify(g => g.GetAllProducts(), Times.Once); //Times: num of times method expected to call
        }
        #endregion GetAllProductTest

        #region GetProductByProductIDTest
        [Test]
        public async Task GetProductByProductID_ValidTest()
        {
            var expectedResult = products[1];
            _productService.Setup(g => g.GetProductByProductID(expectedResult.ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetProductByProductID(expectedResult.ProductID);
            var okResult = actualResult as OkObjectResult;
            var okProductResult = okResult.Value as ProductDto;
            Assert.AreEqual(expectedResult.ID, okProductResult.ID);
            Assert.AreEqual(expectedResult.ProductID, okProductResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, okProductResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, okProductResult.ResellPrice);
            _productService.Verify();
            _productService.Verify(g => g.GetProductByProductID(expectedResult.ProductID), Times.Once); 
        }
        [Test]
        public async Task GetProductByProductID_InvalidTest()
        {
            Guid invalidProductId = new Guid();
            _productService.Setup(g => g.GetProductByProductID(invalidProductId)).ReturnsAsync(null as ProductDto).Verifiable();
            var actualResult = await _mainController.GetProductByProductID(invalidProductId);
            Assert.IsNotNull(actualResult);
            _productService.Verify();
            _productService.Verify(g => g.GetProductByProductID(invalidProductId), Times.Once);
        }
        #endregion GetProductByProductIDTest

        #region GetStockLvlofProductTest
        [Test]
        public async Task GetStockLvlOfProduct_ValidTest()
        {
            int stockLvl = 40;
            _productService.Setup(g => g.GetStockLvlOfProducts(stockLvl)).ReturnsAsync(products).Verifiable();
            var actualResult = await _mainController.GetStockLvlOfProducts(stockLvl);
            var okResult = actualResult as OkObjectResult;
            var productResult = okResult.Value as IEnumerable<ProductDto>;
            var productResultList = productResult.ToList();
            Assert.AreEqual(products.Count, productResultList.Count);
            for (int i = 0; i < products.Count; i++)
            {
                Assert.AreEqual(products[i].ID, productResultList[i].ID);
                Assert.AreEqual(products[i].ProductID, productResultList[i].ProductID);
                Assert.AreEqual(products[i].StockLvl, productResultList[i].StockLvl);
                Assert.AreEqual(products[i].ResellPrice, productResultList[i].ResellPrice);
            }
            _productService.Verify();
            _productService.Verify(g => g.GetStockLvlOfProducts(stockLvl), Times.Once);
        }
        #endregion GetStockLvlofProductTest

       #region GetResellPriceTest
       [Test]
        public async Task GetResellPrice_ValidTest()
        {
            var expectedResult = _resellPrice[1];
            _productService.Setup(g => g.GetResellPriceOfProducts(expectedResult.ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetResellPriceOfProducts(expectedResult.ProductID);
            var okResult = actualResult as OkObjectResult;
            var productResult = okResult.Value as ResellPrice;
            Assert.IsNotNull(productResult);
            Assert.AreEqual(expectedResult.ProductID, productResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrices, productResult.ResellPrices);
            _productService.Verify();
            _productService.Verify(g => g.GetResellPriceOfProducts(expectedResult.ProductID), Times.Once);
        }
        [Test]
        public async Task GetResellPrice_InValidTest()
        {
            Guid invalidProductID = new Guid();
            _productService.Setup(g => g.GetResellPriceOfProducts(invalidProductID)).ReturnsAsync(null as ResellPrice).Verifiable();
            var actualResult = await _mainController.GetResellPriceOfProducts(invalidProductID);
            Assert.IsNotNull(actualResult);
            _productService.Verify();
            _productService.Verify(g => g.GetResellPriceOfProducts(invalidProductID), Times.Once);
        }
        #endregion GetResellPriceTest

        #region SetResellPriceTest
        [Test]
        public async Task SetResellPrice_ValidTest()
        {
            var expectedResult = products[1];
            _productService.Setup(g => g.SetResellPriceofProducts(expectedResult.ID, expectedResult.ResellPrice)).ReturnsAsync(expectedResult).Verifiable();
            objProduct.ProductID = expectedResult.ID;
            objProduct.ResellPrice = expectedResult.ResellPrice;
            var actualResult = await _mainController.SetResellPriceOfProducts(objProduct);
            var okResult = actualResult as OkObjectResult;
            var productResult = okResult.Value as ProductDto;
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(productResult);
            Assert.AreEqual(expectedResult.ID, productResult.ID);
            Assert.AreEqual(expectedResult.ProductID, productResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, productResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, productResult.ResellPrice);
            _productService.Verify();
            _productService.Verify(g => g.SetResellPriceofProducts(expectedResult.ID, expectedResult.ResellPrice), Times.Once);
        }

        public async Task SetResellPrice_InvalidTest()
        {
            Guid invalidProductID = new Guid();
            double resellPrice = 19.99;
            _productService.Setup(g => g.SetResellPriceofProducts(invalidProductID, resellPrice)).ReturnsAsync(null as ProductDto).Verifiable();
            objProduct.ProductID = invalidProductID;
            objProduct.ResellPrice = resellPrice;
            var actualResult = await _mainController.SetResellPriceOfProducts(objProduct);

            Assert.IsNotNull(actualResult);
            _productService.Verify();
            _productService.Verify(g => g.SetResellPriceofProducts(invalidProductID, resellPrice), Times.Once);
        }

        #endregion SetResellPriceTest

        #region SetStockLvlTest 
        [Test]
        public async Task SetStockLvl_ValidTest()
        {
            var expectedResult = products[1];
            _productService.Setup(g => g.SetStockLvlOfProducts(expectedResult.ID, expectedResult.StockLvl)).ReturnsAsync(expectedResult).Verifiable();
            objProduct.ProductID = expectedResult.ID;
            objProduct.StockLvl = expectedResult.StockLvl;
            var actualResult = await _mainController.SetStockLvlOfProducts(objProduct);
            var okResult = actualResult as OkObjectResult;
            var productResult = okResult.Value as ProductDto;
            Assert.AreEqual(expectedResult.ID, productResult.ID);
            Assert.AreEqual(expectedResult.ProductID, productResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, productResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, productResult.ResellPrice);
            _productService.Verify();
            _productService.Verify(g => g.SetStockLvlOfProducts(expectedResult.ID, expectedResult.StockLvl), Times.Once);
        }
        [Test]
        public async Task SetStockLvl_InvalidTest()
        {
            //Invalid product id valid stock lvl 
            Guid invalidID = new Guid();
            int stockLvl = 10;
            objProduct.ProductID = invalidID;
            objProduct.StockLvl = stockLvl;
            var actualResult = await _mainController.SetStockLvlOfProducts(objProduct);
            var result = actualResult as NotFoundResult;
            Assert.AreNotEqual(result.StatusCode, 500);
            _productService.Verify();
            _productService.Verify(g => g.SetStockLvlOfProducts(invalidID, stockLvl), Times.Once);
        }
        #endregion SetStockLvlTest

        #region GetResellHistoryTest
        [Test]
        public async Task GetResellHistoryTest_ValidTest()
        {
            var expectedResult = _resellHistory;
            _productService.Setup(g => g.GetResellHistory(expectedResult[1].ProductID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetResellHistory(expectedResult[1].ProductID);
            var okResult = actualResult as OkObjectResult;
            var okResellHistoryResult = okResult.Value as IEnumerable<ResellHistory>;
            var okResellHistoryResultList = okResellHistoryResult.ToList();
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResellHistoryResult);
            Assert.AreEqual(products.Count, okResellHistoryResultList.Count);
            for (int i = 0; i < _resellHistory.Count; i++)
            {
                Assert.AreEqual(_resellHistory[i].ID, okResellHistoryResultList[i].ID);
                Assert.AreEqual(_resellHistory[i].ProductID, okResellHistoryResultList[i].ProductID);
                Assert.AreEqual(_resellHistory[i].ResellPrice, okResellHistoryResultList[i].ResellPrice);
                Assert.AreEqual(_resellHistory[i].DateTime, okResellHistoryResultList[i].DateTime);
            }
            _productService.Verify();
            _productService.Verify(g => g.GetResellHistory(expectedResult[1].ProductID), Times.Once);
        }
        [Test]
        public async Task GetResellHistoryTest_InvalidTest()
        {
            Guid invalidProductID = new Guid();
            _productService.Setup(g => g.GetResellHistory(invalidProductID)).ReturnsAsync(null as List<ResellHistory>).Verifiable();
            var actualResult = await _mainController.GetResellHistory(invalidProductID);
            Assert.IsNotNull(actualResult);
            _productService.Verify();
            _productService.Verify(g => g.GetResellHistory(invalidProductID), Times.Once);

        }
        #endregion GetResellHistoryTest

        #region GetCustomersTest
        [Test]
        public async Task GetCustomersTest()
        {
            _customerService.Setup(g => g.GetCustomers()).ReturnsAsync(customers).Verifiable();
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
            _customerService.Verify();
            _customerService.Verify(g => g.GetCustomers(), Times.Once); //Times: num of times method expected to call
        }
        #endregion GetCustomersTest

        #region GetCustomerByIDTest
        [Test]
        public async Task GetCustomerByID_ValidTest()
        {
            var expectedResult = customers[1];
            _customerService.Setup(g => g.GetCustomerByID(expectedResult.CustomerID)).ReturnsAsync(expectedResult).Verifiable();
            var actualResult = await _mainController.GetCustomerByID(expectedResult.CustomerID);
            var okResult = actualResult as OkObjectResult;
            var okCustomerResult = okResult.Value as CustomerDto;
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult.CustomerID, okCustomerResult.CustomerID);
            Assert.AreEqual(expectedResult.FirstName, okCustomerResult.FirstName);
            Assert.AreEqual(expectedResult.LastName, okCustomerResult.LastName);
            Assert.AreEqual(expectedResult.Address, okCustomerResult.Address);
            Assert.AreEqual(expectedResult.Postcode, okCustomerResult.Postcode);
            Assert.AreEqual(expectedResult.Email, okCustomerResult.Email);
            Assert.AreEqual(expectedResult.MobNumber, okCustomerResult.MobNumber);
            Assert.AreEqual(expectedResult.PurchaseAbility, okCustomerResult.PurchaseAbility);
           
            _customerService.Verify();
            _customerService.Verify(g => g.GetCustomerByID(expectedResult.CustomerID), Times.Once); //Times: num of times method expected to call
        }
        [Test]
        public async Task GetCustomerByID_InvalidTest()
        {
            Guid invalidProductID = new Guid();
            //should return not found
            _customerService.Setup(g => g.GetCustomerByID(invalidProductID)).ReturnsAsync(null as CustomerDto).Verifiable();
            var actualResult = await _mainController.GetCustomerByID(invalidProductID);
            Assert.IsNotNull(actualResult);
            _customerService.Verify();
            _customerService.Verify(g => g.GetCustomerByID(invalidProductID), Times.Once); //Times: num of times method expected to call
        }

    
        #endregion GetCustomerByIDTest

        #region SetPurchaseAbilityTest
        [Test]
        public async Task SetPurchaseAbilityTest()
        {
            var expectedResult = customers[1];
            _customerService.Setup(g => g.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility)).ReturnsAsync(expectedResult).Verifiable();
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

            _customerService.Verify();
            _customerService.Verify(g => g.SetPurchaseProductAbility(expectedResult.CustomerID, expectedResult.PurchaseAbility), Times.Once); 
        }
        [Test]
        public async Task SetPurchaseAbility_InvalidTest()
        {
            Guid invalidProductID = new Guid();
            _customerService.Setup(g => g.SetPurchaseProductAbility(invalidProductID, true)).ReturnsAsync(null as CustomerDto).Verifiable();
            objCustomer.CustomerID = invalidProductID;
            objCustomer.PurchaseAbility = true;
            var actualResult = await _mainController.SetPurchaseProductAbility(objCustomer);
            var result = actualResult as NotFoundResult;
            Assert.IsNotNull(actualResult);
            _customerService.Verify();
            _customerService.Verify(g => g.SetPurchaseProductAbility(invalidProductID, true), Times.Once);
        }
        #endregion SetPurchaseAbilityTest
    }
}
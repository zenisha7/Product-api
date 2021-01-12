using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Product.Data;
using Product.Models;
using Product.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Tests
{
    public class ProductServiceTests
    {
        private IProductService _product;
        private Mock<ILogger<ProductService>> _logger;
        private Guid productID;
        private ProductDbContext GetProductDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ProductDbContext(options);
            FakeProductService._product.ForEach(g => context.Products.Add(g));
            context.SaveChanges();
            return context;
        }
        [SetUp] 
        public void Setup()
        {
            _logger = new Mock<ILogger<ProductService>>();
            _product = new ProductService(GetProductDbContext(), _logger.Object);
            productID = FakeProductService._product[2].ProductID;
        }

        #region GetAllProductsTest
        [Test]
        public async Task GetAllProductsTest()
        {
            var expectedResult = FakeProductService._product;
            var actualResult = await _product.GetAllProducts();
            Assert.IsInstanceOf<List<ProductDto>>(actualResult);
            for(int i= 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
            }
        }
        #endregion GetAllProductsTest

        #region GetProductByProductIDTest
        //Test for get product Valid Test
        [Test]
        public async Task GetProductByProductID_ValidTest()
        {
            var expectedResult = FakeProductService._product[1];
            var actualResult = await _product.GetProductByProductID(expectedResult.ProductID);
            Assert.IsInstanceOf<ProductDto>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);
        }

        [Test]
        //Test for get product invalid test returns null
        public async Task GetProductByProductID_InvalidTest()
        {
            var invalidProductID = Guid.NewGuid();
            var expectedResult = FakeProductService.GetProductByProductID(invalidProductID);
            var actualResult = await _product.GetProductByProductID(invalidProductID);
            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion GetProductByProductIDTest

        #region GetStockLvlOfProductTest
        [Test]
        public async Task GetStockLvlOfProduct_ValidTest()
        {
            int stockLvl = 30;
            var expectedResult = FakeProductService.GetStockLevelOfProduct(stockLvl);
            var actualResult = await _product.GetStockLvlOfProducts(stockLvl);
            Assert.IsInstanceOf<List<ProductDto>>(actualResult);
            for(int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
            }
        }
        [Test]
        public async Task GetStockLvlOfProduct_InvalidTest()
        {
            //Invalid stock level test. It shouuld return empty.
            int invalidStockLvl = -10;
            var expectedResult = FakeProductService.GetStockLevelOfProduct(invalidStockLvl);
            var actualResult = await _product.GetStockLvlOfProducts(invalidStockLvl);
            Assert.IsInstanceOf<List<ProductDto>>(actualResult);
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
            }
        }

        #endregion GetStockLvlOfProductTest

        #region GetResellPriceTest
        //Test for getting the resell price of stock returns resell price.
        [Test]
        public async Task GetResellPrice_ValidTest()
        {
            var expectedResult = FakeProductService._product[2];
            var actualResult = await _product.GetResellPriceOfProducts(productID);
            Assert.IsInstanceOf<ResellPrice>(actualResult);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrices);
        }

        [Test]
        public async Task GetResellPriceTest_InvalidTest()
        {
            Guid invalidProductID = Guid.NewGuid();
            var expectedResult = FakeProductService.GetResellPriceOfProducts(invalidProductID);
            var actualResult = await _product.GetResellPriceOfProducts(invalidProductID);
            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion GetResellPriceTest

        #region GetResellHistoryTest
        //Test for getting the resell history of products.
        [Test]
        public async Task GetResellHistory_ValidTest()
        {
            var expectedResult = FakeProductService.GetResellHistory(productID);
            var actualResult = await _product.GetResellHistory(productID);
            Assert.IsInstanceOf<List<ResellHistory>>(actualResult);
            for(int i =0; i<actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].DateTime, actualResult[i].DateTime);
            }
        }
        [Test]
        //Resell history test when productId is invalid, should return empty.
        public async Task GetResellHistory_InvalidTest()
        {
            Guid invalidProductID = new Guid();
            var expectedResult = FakeProductService.GetResellHistory(invalidProductID);
            var actualResult = await _product.GetResellHistory(productID);
            Assert.IsInstanceOf<List<ResellHistory>>(actualResult);
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].DateTime, actualResult[i].DateTime);
            }
        }

        #endregion GetResellHistoryTest

        #region SetStockLvlTest
        //Test for setting the stock level of productt.
        [Test]
        public async Task SetStockLvlOfProduct_ValidTest()
        {
            var expectedResult = FakeProductService._product[1];
            var actualResult = await _product.SetStockLvlOfProducts(expectedResult.ProductID, expectedResult.StockLvl);
            Assert.IsInstanceOf<ProductDto>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);
        }
        public async Task SetStockLvlOfProduct_InvalidTest()
        {
            var invalidProductID = Guid.NewGuid();
            int stockLvl = 50;
            var expectedResult = FakeProductService.SetStockLevelOfProduct(invalidProductID, stockLvl);
            var actualResult = await _product.SetStockLvlOfProducts(invalidProductID, stockLvl);
            Assert.AreEqual(expectedResult, actualResult);
        }
#endregion SetStockLvlTest
    }
}
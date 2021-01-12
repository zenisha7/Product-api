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
        private int stockLvl;
        private double resellPrice;
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
            productID = FakeProductService._product[1].ProductID;
            stockLvl = 30;
            resellPrice = FakeProductService._product[1].ResellPrice;
        }


        #region GetAllProductsTest
        [Test]
        public async Task GetAllProductsTest()
        {
            var expectedResult = FakeProductService._product;
            var actualResult = await _product.GetAllProducts();
            Assert.IsNotNull(actualResult);
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

        #region GetStockByIDTest
        //Test for get stock by its product ID
        [Test]
        public async Task GetStockByIDTest()
        {
            var expectedResult = FakeProductService._product[1];
            var actualResult = await _product.GetProductByProductID(expectedResult.ProductID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<ProductDto>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);

        }
        #endregion GetStockByIDTest

        #region GetStockByLvlTest
        //Getting stock by stock level
        [Test]
        public async Task GetStockByLvlTest()
        {
            var expectedResult = FakeProductService.GetStockLevelOfProduct(stockLvl);
            var actualResult = await _product.GetStockLvlOfProducts(stockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<ProductDto>>(actualResult);
            for(int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
                
            }
        }
        #endregion GetStockByLvlTest

        #region GetResellPriceTest
        //Test for getting the resell price of stock.
        [Test]
        public async Task GetResellPriceTest()
        {
            var expectedResult = FakeProductService._resellPrice[1];
            var actualResult = await _product.GetResellPriceOfProducts(expectedResult.ProductID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<ResellPrice>(actualResult);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrices, actualResult.ResellPrices);
        }
        #endregion GetResellPriceTest

        #region GetResellHistoryTest
        //Test for getting the resell history of stock.
        [Test]
        public async Task GetResellHistoryTest()
        {
            var expectedResult = FakeProductService.GetResellHistory(productID);
            var actualResult = await _product.GetResellHistory(productID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<ResellHistory>>(actualResult);
            for(int i =0; i<actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].DateTime, actualResult[i].DateTime);

            }
        }
        #endregion GetResellHistoryTest

        #region SetStockLvlTest
        //Test for setting the stock level of stock.
        [Test]
        public async Task SetStockLvlTest()
        {
            var expectedResult = FakeProductService._product[1];
            var actualResult = await _product.SetStockLvlOfProducts(expectedResult.ProductID, expectedResult.StockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<ProductDto>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);

        }
# endregion SetStockLvlTest
    }
}
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
    public class StockServicesTests
    {
        private IStockService _stockRepo;
        private Mock<ILogger<StockServices>> _logger;
        private Guid productID;
        private int stockLvl;
        private double resellPrice;
        private ProductDbContext GetProductDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ProductDbContext(options);
            FakeStockService._stock.ForEach(g => context.Stocks.Add(g));
            context.SaveChanges();
            return context;
        }
        [SetUp] 
        public void Setup()
        {
            _logger = new Mock<ILogger<StockServices>>();
            _stockRepo = new StockServices(GetProductDbContext(), _logger.Object);
            productID = FakeStockService._stock[1].ProductID;
            stockLvl = 30;
            resellPrice = FakeStockService._stock[1].ResellPrice;
        }

      
        #region GetStockTest
        [Test]
        public async Task GetStockTest()
        {
            var expectedResult = FakeStockService._stock;
            var actualResult = await _stockRepo.GetStock();
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<Stock>>(actualResult);
            for(int i= 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
            }
        }
        #endregion GetStockTest

        #region GetStockByIDTest
        //Test for get stock by its product ID
        [Test]
        public async Task GetStockByIDTest()
        {
            var expectedResult = FakeStockService._stock[1];
            var actualResult = await _stockRepo.GetStockByProductID(expectedResult.ProductID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Stock>(actualResult);
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
            var expectedResult = FakeStockService.GetStockByStockLevel(stockLvl);
            var actualResult = await _stockRepo.GetStockByStockLvl(stockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<List<Stock>>(actualResult);
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
            var expectedResult = FakeStockService._resellPrice[1];
            var actualResult = await _stockRepo.GetStockResellPrice(expectedResult.ProductID);
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
            var expectedResult = FakeStockService.GetResellHistory(productID);
            var actualResult = await _stockRepo.GetResellHistory(productID);
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
            var expectedResult = FakeStockService._stock[1];
            var actualResult = await _stockRepo.SetStockLvl(expectedResult.ProductID, expectedResult.StockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Stock>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);

        }
# endregion SetStockLvlTest
    }
}
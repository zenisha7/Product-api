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
    public class StockRepositoryTests
    {
        private IStockRepository _stockRepo;
        private Mock<ILogger<StockRepository>> _logger;
        private Guid productID;
        private int stockLvl;
        private double resellPrice;
        private ProductDbContext GetProductDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ProductDbContext(options);
            MockStockRepository._stock.ForEach(g => context.Stocks.Add(g));
            context.SaveChanges();
            return context;
        }
        [SetUp] 
        public void Setup()
        {
            _logger = new Mock<ILogger<StockRepository>>();
            _stockRepo = new StockRepository(GetProductDbContext(), _logger.Object);
            productID = MockStockRepository._stock[1].ProductID;
            stockLvl = MockStockRepository._stock[1].StockLvl;
            resellPrice = MockStockRepository._stock[1].ResellPrice;
        }

        [Test]
        public async Task GetStock()
        {
            var expectedResult = MockStockRepository._stock;
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

        //Test for get stock by its product ID
        [Test]
        public async Task GetStockByID()
        {
            var expectedResult = MockStockRepository._stock[1];
            var actualResult = await _stockRepo.GetStockByProductID(expectedResult.ProductID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Stock>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);

        }

        //Getting stock by stock level
        [Test]
        public async Task GetStockByLvl()
        {
            var expectedResult = MockStockRepository.GetStockByStockLevel(stockLvl);
            var actualResult = await _stockRepo.GetStockByStockLvl(stockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Stock>(actualResult);
            for(int i = 0; i < actualResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, actualResult[i].ID);
                Assert.AreEqual(expectedResult[i].ProductID, actualResult[i].ProductID);
                Assert.AreEqual(expectedResult[i].StockLvl, actualResult[i].StockLvl);
                Assert.AreEqual(expectedResult[i].ResellPrice, actualResult[i].ResellPrice);
            }
        }

        //Test for getting the resell price of stock.
        [Test]
        public async Task GetResellPrice()
        {
            var expectedResult = MockStockRepository._resellPrice[1];
            var actualResult = await _stockRepo.GetStockResellPrice(expectedResult.ProductID);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<ResellPrice>(actualResult);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrices, actualResult.ResellPrices);
        }

        //Test for getting the resell history of stock.
        [Test]
        public async Task GetResellHistory()
        {
            var expectedResult = MockStockRepository.GetResellHistory(productID);
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

        //Test for setting the stock level of stock.
        [Test]
        public async Task SetStockLvl()
        {
            var expectedResult = MockStockRepository._stock[1];
            var actualResult = await _stockRepo.SetStockLvl(expectedResult.ProductID, expectedResult.StockLvl);
            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<Stock>(actualResult);
            Assert.AreEqual(expectedResult.ID, actualResult.ID);
            Assert.AreEqual(expectedResult.ProductID, actualResult.ProductID);
            Assert.AreEqual(expectedResult.ResellPrice, actualResult.ResellPrice);
            Assert.AreEqual(expectedResult.StockLvl, actualResult.StockLvl);

        }

        




    }
}
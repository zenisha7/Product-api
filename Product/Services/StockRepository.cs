using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Models;
using Product.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Product.Services
{
    public class StockRepository : IStockRepository
    {
        private readonly ProductDbContext _dbcontext;
        private readonly ILogger<StockRepository> _logger;

        public StockRepository(ProductDbContext dbContext, ILogger<StockRepository> logger)
        {
            _dbcontext = dbContext;
            _logger = logger;
        }

        public async Task<List<Stock>> GetStock()
        {
            try
            {
                Console.WriteLine("Getting all stock");
                var getStock = await _dbcontext.Stocks.ToListAsync();
                _logger.LogInformation($"Getting total of {getStock.Count} stock.");
                return await Task.FromResult(getStock);
            }catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<Stock> GetStockByProductID(Guid productID)
        {
            try
            {
                Console.WriteLine("Getting all stock with their product ID.");
                var getStock = _dbcontext.Stocks.Where(g => g.ProductID == productID).FirstOrDefault();
                _logger.LogInformation($"Getting stock with their product ID {getStock.ProductID}.");
                return await Task.FromResult(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<Stock>> GetStockByStockLvl(int stockLvl)
        {
            try
            {
                Console.WriteLine("Getting all stock by the level of stock.");
                var getStock = _dbcontext.Stocks.Where(g => g.StockLvl == stockLvl).ToList();
                _logger.LogInformation($"Getting stock by the level of stock {stockLvl}.");
                return await Task.FromResult(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<ResellPrice> GetStockResellPrice(Guid productID)
        {
            try
            {
                Console.WriteLine("Getting all stock by the level of stock.");
                var getStock = _dbcontext.Stocks.Select(g => new ResellPrice
                {
                    ProductID = g.ProductID,
                    ResellPrices = g.ResellPrice
                }).Where(g => g.ProductID == productID).FirstOrDefault();
                
                
                _logger.LogInformation($"Returns resell price of stock with product ID {getStock.ProductID}");
                return await Task.FromResult(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }
        public async Task<Stock> SetStockResellPrice(Guid productID, double resellPrice)
        {
            try
            {
                Console.WriteLine("Getting stock with product ID.");
                var getStock = await _dbcontext.Stocks.Where(g => g.ProductID == productID).FirstOrDefaultAsync();
                
                 if(getStock != null  && resellPrice >= 0)
                {
                    _logger.LogInformation($"Setting the resell price of stock with product ID {getStock.ProductID} to {resellPrice}");
                    getStock.ResellPrice = resellPrice;
                    _dbcontext.Stocks.Update(getStock);

                    var getResellHistory = new ResellHistory
                    {
                        ID = Guid.NewGuid(),
                        ProductID = productID,
                        ResellPrice = resellPrice,
                        DateTime = DateTime.UtcNow
                    };
                    _dbcontext.ResellHistories.Add(getResellHistory);
                    await _dbcontext.SaveChangesAsync();
                }

                _logger.LogInformation($"Returns stock with product ID {getStock.ProductID} with new resell price {getStock.ResellPrice}");
                return await Task.FromResult(getStock);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }
        public async Task<Stock> SetStockLvl(Guid productID, int stockLvl)
        {
            try
            {
                Console.WriteLine("Getting stock with their product ID.");
                var getStock = await _dbcontext.Stocks.Where(g => g.ProductID == productID).FirstOrDefaultAsync();
                if(getStock != null && stockLvl >= 0 )
                {
                    Console.WriteLine("Setting stock level of stock.");
                    getStock.StockLvl = stockLvl;
                    _dbcontext.Stocks.Update(getStock);
                    await _dbcontext.SaveChangesAsync();
                }

                _logger.LogInformation($"Returns stock with product ID {getStock.ProductID} with its new stock level {getStock.StockLvl}");
                return await Task.FromResult(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }
        public async Task<List<ResellHistory>> GetResellHistory(Guid productID)
        {
            try
            {
                Console.WriteLine("Getting the resell history of stock.");
                var getResellHistory = await _dbcontext.ResellHistories.Where(g => g.ProductID == productID).ToListAsync();
                return await Task.FromResult(getResellHistory);
            }catch(Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
            
        }
    }
}
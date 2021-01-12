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
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _dbcontext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbcontext = dbContext;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            try
            {
                var getProducts = await _dbcontext.Products.ToListAsync();
                _logger.LogInformation($"Getting total of {getProducts.Count} products.");
                return await Task.FromResult(getProducts);
            }catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductDto> GetProductByProductID(Guid productID)
        {
            try
            {
                var getProducts = _dbcontext.Products.Where(g => g.ProductID == productID).FirstOrDefault();
                _logger.LogInformation($"Getting stock with their product ID {getProducts.ProductID}.");
                return await Task.FromResult(getProducts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<ProductDto>> GetStockLvlOfProducts(int stockLvl)
        {
            try
            {
                var product = _dbcontext.Products.Where(g => g.StockLvl == stockLvl).ToList();
                _logger.LogInformation($"Getting stock by the level of products {stockLvl}.");
                return await Task.FromResult(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }

        public async Task<ResellPrice> GetResellPriceOfProducts(Guid productID)
        {
            try
            {
                var product = _dbcontext.Products.Select(g => new ResellPrice
                {
                    ProductID = g.ProductID,
                    ResellPrices = g.ResellPrice
                }).Where(g => g.ProductID == productID).FirstOrDefault();
                
                _logger.LogInformation($"Returns resell price of stock with product ID {product.ProductID}");
                return await Task.FromResult(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }
        public async Task<ProductDto> SetResellPriceofProducts(Guid productID, double resellPrice)
        {
            try
            {
                _logger.LogInformation("Getting products with product ID.");
                var product = await _dbcontext.Products.Where(g => g.ProductID == productID).FirstOrDefaultAsync();
                 if(product != null  && resellPrice >= 0)
                {
                    _logger.LogInformation($"Setting the resell price of product with product ID {product.ProductID} to {resellPrice}");
                    product.ResellPrice = resellPrice;
                    _dbcontext.Products.Update(product);

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
                _logger.LogInformation($"Returns product with product ID {product.ProductID} with new resell price {product.ResellPrice}");
                return await Task.FromResult(product);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }
        public async Task<ProductDto> SetStockLvlOfProducts(Guid productID, int stockLvl)
        {
            try
            {
                _logger.LogInformation("Getting products with their product ID.");
                var product = await _dbcontext.Products.Where(g => g.ProductID == productID).FirstOrDefaultAsync();
                if(product != null && stockLvl >= 0 )
                {
                    _logger.LogInformation("Setting stock level of products.");
                    product.StockLvl = stockLvl;
                    _dbcontext.Products.Update(product);
                    await _dbcontext.SaveChangesAsync();
                }
                _logger.LogInformation($"Returns stock with product ID {product.ProductID} with its new stock level {product.StockLvl}");
                return await Task.FromResult(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
        }
        public async Task<List<ResellHistory>> GetResellHistory(Guid productID)
        {
            try
            {
                Console.WriteLine("Getting the resell history of products.");
                var getResellHistory = await _dbcontext.ResellHistories.Where(g => g.ProductID == productID).ToListAsync();
                return await Task.FromResult(getResellHistory);
            }catch(Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return null;
            }
            
        }
    }
}
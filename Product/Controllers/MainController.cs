using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Product.Models;
using Product.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Polly;
namespace Product.Controllers
{
    public class MainController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<MainController> _logger;

        public MainController(ICustomerRepository customerRepository, IStockRepository stockRepository, ILogger<MainController> logger)
        {
            _customerRepository = customerRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task<IActionResult> GetStock()
        {
            try
            {
                Console.WriteLine("Try to get all stocks");
                var stock = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStock())
                    .ConfigureAwait(false);
                _logger.LogInformation($"Getting {stock.Count} stock");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        public async Task<IActionResult> GetStockByProductID(Guid productID)
        {
            try
            {
                Console.WriteLine("Try to get stock by its product ID " + productID);
                var stock = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockByProductID(productID))
                    .ConfigureAwait(false);

                if (stock == null)
                {
                    Console.WriteLine("Stock  not found");
                    return NotFound();
                }

                _logger.LogInformation($"Retriving stock with product ID {stock.ProductID} ");
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        public async Task<IActionResult> GetStockByStockLvl(int stockLvl)
        {
            try
            {
                if (stockLvl < 0)
                {
                    Console.WriteLine("Invalid. The stock level cannot be less than 0.");
                    return BadRequest();

                }
                Console.WriteLine("Getting stock by stock level");

                var stock = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockByStockLvl(stockLvl))
                    .ConfigureAwait(false);

                _logger.LogInformation($"Get stock by stock level {stockLvl} ");
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        public async Task<IActionResult> GetStockResellPrice(Guid productID)
        {
            try
            {

                Console.WriteLine("Trying to get resell price of stock with their product ID");

                var stock = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockResellPrice(productID))
                    .ConfigureAwait(false);
                if (stock == null)
                {
                    Console.WriteLine("Stock  not found");
                    return NotFound();
                }

                _logger.LogInformation($"Retriving resell price of stock with {productID} as product ID.");
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }

        }

        public async Task<IActionResult> SetStockResellPrice([Bind("ProductID, ResellPrice")]Stock ObjStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Constructed an invalid stock object.");
                    return NoContent();
                }

                if (ObjStock.ResellPrice < 0)
                {
                    Console.WriteLine(ObjStock.ResellPrice + " is not valid. The resell price cannot be less than 0.");
                    return BadRequest();
                }

                _logger.LogInformation($"Setting {ObjStock.ResellPrice} as resell price of stock with product ID {ObjStock.ProductID}");

                var stock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.SetStockResellPrice(ObjStock.ProductID, ObjStock.ResellPrice))
                    .ConfigureAwait(false);
                if (stock == null)
                {
                    Console.WriteLine("Stock having product ID" + ObjStock.ProductID + "not found.");
                    return NotFound();
                }

                Console.WriteLine(" New resell price as " + ObjStock.ResellPrice + "is set for stock with product ID " + ObjStock.ProductID);
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }

        }

        public async Task<IActionResult> SetStockLvl([Bind("ProductID, StockLvl")] Stock ObjStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Constructed an invalid stock object.");
                    return NoContent();
                }

                if (ObjStock.StockLvl < 0)
                {
                    Console.WriteLine(ObjStock.StockLvl + " is not valid. The stock level cannot be less than 0.");
                    return BadRequest();
                }

                _logger.LogInformation($"Setting {ObjStock.StockLvl} as stock level of stock having product ID {ObjStock.ProductID}");

                var stock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.SetStockLvl(ObjStock.ProductID, ObjStock.StockLvl))
                    .ConfigureAwait(false);
                if (stock == null)
                {
                    Console.WriteLine("Stock having product ID" + ObjStock.ProductID + "not found.");
                    return NotFound();
                }

                Console.WriteLine(" New stock level as" + ObjStock.StockLvl + "is set for stock with product ID " + ObjStock.ProductID);
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        public async Task<IActionResult> GetStockResellHistory(Guid productID)
        {
            try
            {

                Console.WriteLine("Getting resell history of stock with product ID " + productID);

                var stock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetResellHistory(productID))
                    .ConfigureAwait(false);
                if (stock == null)
                {
                    Console.WriteLine("Stock  not found");
                    return NotFound();
                }

                _logger.LogInformation($"Getting resell history of stock with {productID} as product ID.");
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}\nStack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

///////////////////////////////////////////////////// Customer Methods \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


    }



}


    

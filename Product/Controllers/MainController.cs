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
using Microsoft.AspNetCore.Authorization;

namespace Product.Controllers
{
    [Route("")]
    [ApiController]
    [Authorize(Policy = "Staff")]
    public class MainController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<MainController> _logger;

        public MainController( ICustomerRepository customerRepository, IStockRepository stockRepository, ILogger<MainController> logger)
        {
            _customerRepository = customerRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }


        public async Task<IActionResult> GetStock()
        {
            try
            {
                //Trying to get all stocks
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
                //Attempting to get stock by its product ID

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
               _logger.LogInformation("Getting stock by stock level");

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

               //Attempting to get resell price of stock with their product ID
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
                    _logger.LogInformation($"Stock having product ID {ObjStock.ProductID} not found.");
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

        #region 2

        ///////////////////////////////////////////////////// Customer Methods \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                //Trying to get all the customers
                var getCustomers = await Policy.Handle<Exception>()
                    .RetryAsync(2).ExecuteAsync(async () => await _customerRepository.GetCustomers(/*HttpToken*/))
                    .ConfigureAwait(false);
                _logger.LogInformation($"Getting {getCustomers.Count} customers");
                return Ok(getCustomers);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Error message {ex.Message} \n Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> GetCustomerByID(Guid customerID)
        {
            try
            {
                //Trying to get customers woth their ID.
                var getCustomer = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _customerRepository.GetCustomerByID(customerID))
                    .ConfigureAwait(false);

                if (getCustomer == null)
                {
                    _logger.LogInformation($"The customer id {customerID}, not found");
                    return NotFound();
                }
                //returning customer with ID
                return Ok(getCustomer);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Error message {ex.Message} \n Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> SetPurchaseProductAbility ([Bind("customerID, purchaseAbility")]Customer objCustomer)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    Console.WriteLine("Invalid object of customer was constructed");
                    return NoContent();
                }
                //Trying to set purchase ability of customer.
                var getCustomer = await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () => await _customerRepository.SetPurchaseProductAbility(objCustomer.CustomerID, objCustomer.PurchaseAbility))
                    .ConfigureAwait(false);

                if (getCustomer == null)
                {
                    _logger.LogInformation($"The customer id {objCustomer.CustomerID}, not found");
                    return NotFound();
                }
                _logger.LogInformation($"Returns customer with ID{objCustomer.CustomerID} and its purchase product ability{objCustomer.PurchaseAbility}.");
                return Ok(getCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message {ex.Message} \n Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        #endregion 2
    }



}


    

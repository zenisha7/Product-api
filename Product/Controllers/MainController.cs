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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Staff")]
    public class MainController : Controller
    {
        private readonly ICustomerService _customerRepository;
        private readonly IStockService _stockRepository;
        private readonly ILogger<MainController> _logger;

        public MainController( ICustomerService customerRepository, IStockService stockRepository, ILogger<MainController> logger)
        {
            _customerRepository = customerRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        #region Internal Service Methods
        [HttpGet("api/Main/GetStock")]
        //api/Main/GetStock
        public async Task<IActionResult> GetStock()
        {
            try
            {
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStock())
                    .ConfigureAwait(false);
                _logger.LogInformation($"Getting {getStock.Count} stock");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetStockByProductID
        public async Task<IActionResult> GetStockByProductID(Guid productID)
        {
            try
            {
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockByProductID(productID))
                    .ConfigureAwait(false);
                if (getStock == null)
                {
                    _logger.LogInformation($"Stock not found. ID num: {productID} ");
                    return NotFound();
                }
                _logger.LogInformation($"Retriving stock with product ID {getStock.ProductID} ");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetStockByStockLvl
        public async Task<IActionResult> GetStockByStockLvl(int stockLvl)
        {
            try
            {
                if (stockLvl < 0)
                {
                    _logger.LogInformation($"Invalid level of stock: {stockLvl}");
                    return BadRequest();
                }
                //Getting stock by stock level
                var stock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockByStockLvl(stockLvl))
                    .ConfigureAwait(false);
                _logger.LogInformation($"Returns stock by stock level {stockLvl}");
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetStockResellPrice
        public async Task<IActionResult> GetStockResellPrice(Guid productID)
        {
            try
            {
               //Getting resell price of stock with their product ID
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetStockResellPrice(productID))
                    .ConfigureAwait(false);
                if (getStock == null)
                {
                    return NotFound();
                }
                _logger.LogInformation($"Returns resell price of stock with {productID} as product ID.");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }

        }

        // api/Main/SetStockResellPrice
        [HttpPost()]
        public async Task<IActionResult> SetStockResellPrice([Bind("ProductID, ResellPrice")]Stock ObjStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("The constructed object of stock is not valid.");
                    return NoContent();
                }
                if (ObjStock.ResellPrice < 0)
                {
                    _logger.LogInformation($"Resell price can not be les than 0. : {ObjStock.ResellPrice}");
                    return BadRequest();
                }
                _logger.LogInformation($"Setting {ObjStock.ResellPrice} as resell price of stock with product ID {ObjStock.ProductID}");
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.SetStockResellPrice(ObjStock.ProductID, ObjStock.ResellPrice))
                    .ConfigureAwait(false);
                if (getStock == null)
                {
                    _logger.LogInformation($"Stock not found.");
                    return NotFound();
                }
                _logger.LogInformation($"New resell price as {ObjStock.ResellPrice}is set for stock with product ID  {ObjStock.ProductID}");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/SetStockLvl
        [HttpPost()]
        public async Task<IActionResult> SetStockLvl([Bind("ProductID, StockLvl")] Stock ObjStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("The constructed object of stock is not valid.");
                    return NoContent();
                }
                if (ObjStock.StockLvl < 0)
                {
                    _logger.LogInformation($"Stock level can not be les than 0. : {ObjStock.StockLvl}");
                    return BadRequest();
                }
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.SetStockLvl(ObjStock.ProductID, ObjStock.StockLvl))
                    .ConfigureAwait(false);
                if (getStock == null)
                {
                    _logger.LogInformation($"Stock having product ID {ObjStock.ProductID} not found.");
                    return NotFound();
                }
                _logger.LogInformation($" New stock level as {ObjStock.StockLvl} is set for stock with product ID {ObjStock.ProductID}");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }
        //api/Main/GetStockResellHistory
        public async Task<IActionResult> GetStockResellHistory(Guid productID)
        {
            try
            {
                //Gets resell history of stock with resell price.
                var getStock = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _stockRepository.GetResellHistory(productID))
                    .ConfigureAwait(false);
                if (getStock == null)
                {
                    _logger.LogInformation("Stock  not found");
                    return NotFound();
                }
                _logger.LogInformation($"Getting resell history of stock with {productID} as product ID.");
                return Ok(getStock);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Server error");
            }
        }
        #endregion Internal Service Methods

        #region External Service Methods
        ///////////////////////////////////////////////////// Customer Methods \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        //api/Main/GetCustomers
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                //Trying to get all the customers
                //for intregation do:
                //var httpToken = HttpContext.Request.Headers[" "].ToString();
                var getCustomers = await Policy.Handle<Exception>()
                    .RetryAsync(2).ExecuteAsync(async () => await _customerRepository.GetCustomers(/*HttpToken*/))
                    .ConfigureAwait(false);
                _logger.LogInformation($"Getting {getCustomers.Count} customers");
                return Ok(getCustomers);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }
        //api/Main/GetCustomerByID
        public async Task<IActionResult> GetCustomerByID(Guid customerID)
        {
            try
            {
                //Trying to get customers woth their ID.
                var getCustomer = await Policy.Handle<Exception>().RetryAsync(2)
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
                _logger.LogInformation($"Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        //api/Main/SetPurchaseProductAbility
        public async Task<IActionResult> SetPurchaseProductAbility ([Bind("customerID, purchaseAbility")]Customer objCustomer)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogInformation("The constructed object of customer is not valid.");
                    return NoContent();
                }
                //Trying to set purchase ability of customer.
                var getCustomer = await Policy.Handle<Exception>().RetryAsync(2)
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
                _logger.LogInformation($"Stack trace {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        #endregion External Service Methods
    }

}


    

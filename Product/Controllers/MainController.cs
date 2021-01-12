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
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ILogger<MainController> _logger;

        public MainController( ICustomerService customerService, IProductService productService, ILogger<MainController> logger)
        {
            _customerService = customerService;
            _productService = productService;
            _logger = logger;
        }

        #region Internal Service Methods
        [HttpGet("api/Main/GetAllProducts")]
        //api/Main/GetAllProducts
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var getProducts = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.GetAllProducts())
                    .ConfigureAwait(false);
                _logger.LogInformation($"Getting {getProducts.Count} product.");
                return Ok(getProducts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetProductByProductID
        public async Task<IActionResult> GetProductByProductID(Guid productID)
        {
            try
            {
                var getProduct = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.GetProductByProductID(productID))
                    .ConfigureAwait(false);
                if (getProduct == null)
                {
                    _logger.LogInformation($"Product not found. Product ID: {productID} ");
                    return NotFound();
                }
                _logger.LogInformation($"Returning product with product ID {getProduct.ProductID} ");
                return Ok(getProduct);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetStockLvlOfProducts
        public async Task<IActionResult> GetStockLvlOfProducts(int stockLvl)
        {
            try
            {
                if (stockLvl < 0)
                {
                    _logger.LogInformation($"Invalid level of stock: {stockLvl}");
                    return BadRequest();
                }
                //Getting stock by stock level
                var getProduct = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.GetStockLvlOfProducts(stockLvl))
                    .ConfigureAwait(false);
                _logger.LogInformation($"Returns stock level {stockLvl} of products.");
                return Ok(getProduct);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/GetResellPriceOfProducts
        public async Task<IActionResult> GetResellPriceOfProducts(Guid productID)
        {
            try
            {
               //Getting resell price of product with their product ID
                var getProduct = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.GetResellPriceOfProducts(productID))
                    .ConfigureAwait(false);
                if (getProduct == null)
                {
                    return NotFound();
                }
                _logger.LogInformation($"Returns resell price of stock with {productID} as product ID.");
                return Ok(getProduct);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }

        }

        // api/Main/SetResellPriceOfProducts
        [HttpPost()]
        public async Task<IActionResult> SetResellPriceOfProducts([Bind("ProductID, ResellPrice")]ProductDto objProduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Invalid construction of product object.");
                    return NoContent();
                }
                if (objProduct.ResellPrice < 0)
                {
                    _logger.LogInformation($"Resell price : {objProduct.ResellPrice}, Resell price should not be less than 0");
                    return BadRequest();
                }
                _logger.LogInformation($"Setting {objProduct.ResellPrice} as resell price of product with product ID {objProduct.ProductID}");
                var getProduct = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.SetResellPriceofProducts(objProduct.ProductID, objProduct.ResellPrice))
                    .ConfigureAwait(false);
                if (getProduct == null)
                {
                    _logger.LogInformation($"Product having product ID {objProduct.ProductID} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"New resell price as {objProduct.ResellPrice}is set for product with product ID  {objProduct.ProductID}");
                return Ok(getProduct);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }
        }

        //api/Main/SetStockLvlOfProducts
        [HttpPost()]
        public async Task<IActionResult> SetStockLvlOfProducts([Bind("ProductID, StockLvl")] ProductDto objProducts)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Invalid construction of product object.");
                    return NoContent();
                }
                if (objProducts.StockLvl < 0)
                {
                    _logger.LogInformation($"Stock level of products cannot be les than 0. : {objProducts.StockLvl}");
                    return BadRequest();
                }
                var getProducts = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.SetStockLvlOfProducts(objProducts.ProductID, objProducts.StockLvl))
                    .ConfigureAwait(false);
                if (getProducts == null)
                {
                    _logger.LogInformation($"Product having product ID {objProducts.ProductID} not found.");
                    return NotFound();
                }
                _logger.LogInformation($" New stock level as {objProducts.StockLvl} is set for the product with product ID {objProducts.ProductID}");
                return Ok(getProducts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500, "Server error");
            }
        }
        //api/Main/GetResellHistory
        public async Task<IActionResult> GetResellHistory(Guid productID)
        {
            try
            {
                //Gets resell history of stock with resell price.
                var getProducts = await Policy.Handle<Exception>().RetryAsync(2)
                    .ExecuteAsync(async () => await _productService.GetResellHistory(productID))
                    .ConfigureAwait(false);
                if (getProducts == null)
                {
                    _logger.LogInformation("Product not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Getting resell history of product with {productID} as product ID.");
                return Ok(getProducts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error message: {ex.Message}");
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
                    .RetryAsync(2).ExecuteAsync(async () => await _customerService.GetCustomers(/*HttpToken*/))
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
                    .ExecuteAsync(async () => await _customerService.GetCustomerByID(customerID))
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
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500);
            }
        }

        //api/Main/SetPurchaseProductAbility
        public async Task<IActionResult> SetPurchaseProductAbility ([Bind("customerID, purchaseAbility")]CustomerDto objCustomer)
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
                    .ExecuteAsync(async () => await _customerService.SetPurchaseProductAbility(objCustomer.CustomerID, objCustomer.PurchaseAbility))
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
                _logger.LogInformation($"Error message: {ex.Message}");
                return StatusCode(500);
            }
        }

        #endregion External Service Methods
    }

}


    

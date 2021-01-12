using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts(); //Gets all the stock in a list
        Task<ProductDto> GetProductByProductID(Guid productID); //Get stocks by their product Id.
        Task<List<ProductDto>> GetStockLvlOfProducts(int stockLevel);
        Task<ResellPrice> GetResellPriceOfProducts(Guid productID);
        Task<ProductDto> SetResellPriceofProducts(Guid productID, double resellPrice);
        Task<ProductDto> SetStockLvlOfProducts(Guid productID, int stockLvl);
        Task<List<ResellHistory>> GetResellHistory(Guid productID);
    }
}
 
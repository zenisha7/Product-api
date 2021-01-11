using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public interface IStockService
    {
        Task<List<Stock>> GetStock(); //Gets all the stock in a list
        Task<Stock> GetStockByProductID(Guid productID); //Get stocks by their product Id.
        Task<List<Stock>> GetStockByStockLvl(int stockLevel);
        Task<ResellPrice> GetStockResellPrice(Guid productID);
        Task<Stock> SetStockResellPrice(Guid productID, double resellPrice);
        Task<Stock> SetStockLvl(Guid productID, int stockLvl);
        Task<List<ResellHistory>> GetResellHistory(Guid productID);
    }
}
 
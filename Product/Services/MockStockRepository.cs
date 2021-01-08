using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public class MockStockRepository
    {
        public static readonly List<Stock> _stock = new List<Stock>
        {
            new Stock{ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                StockLvl = 123,
                ResellPrice = 100.50,
            },
            new Stock {ID = new Guid("6de2ebdc-d39a-4f12-b7d9-44453833bfee"),
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                StockLvl = 456,
                ResellPrice = 200.50,
            },
            new Stock {ID = new Guid("36004789-ebe9-47f9-a45c-6502232e5304"),
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                StockLvl = 789,
                ResellPrice = 300.50,
            }
        };

        public static readonly List<ResellPrice> _resellPrice = new List<ResellPrice>
        {
            new ResellPrice {
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrices = 100.50,
            },
            new ResellPrice {
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                ResellPrices = 200.50,
            },
            new ResellPrice {
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                ResellPrices = 300.50,
            }
        };

        public static readonly List<ResellHistory> _resellHistory = new List<ResellHistory>
        {
            new ResellHistory {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 1000.50,
                DateTime = new DateTime(2019, 12, 2, 12, 00, 00),
            },
            new ResellHistory {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 500.50,
                DateTime = new DateTime(2019, 12, 2, 12, 30, 00),
            },
            new ResellHistory {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 100.50,
                DateTime = new DateTime(2019, 12, 2, 13, 00, 00),
            },
        };

        public static Stock GetStockByProductID(Guid productID)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();
            return stock;
        }

        public static List<Stock> GetStockByStockLevel(int stockLevel)
        {
            var stock = _stock.Where(s => s.StockLvl <= stockLevel).ToList();
            return stock;
        }

        public static ResellPrice GetResellPrice(Guid productID)
        {
            var resellPrice= _resellPrice.Where(r => r.ProductID == productID).FirstOrDefault();
            return resellPrice;
        }

        public static List<ResellHistory> GetResellHistory(Guid productID)
        {
            var resellHistory = _resellHistory.Where(r => r.ProductID == productID).ToList();
            return resellHistory;
        }

        public static Stock SetResellPrice(Guid productID, double resellPrice)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();
            if (stock != null && resellPrice >= 0)
            {
                stock.ResellPrice = resellPrice;
            }
            return stock;
        }

        public static Stock SetStockLevel(Guid productID, int stockLevel)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();
            if (stock != null && stockLevel >=0)
            {
                stock.StockLvl = stockLevel;
            }
            return stock;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Models;

namespace Product.Services
{
    public class FakeProductService
    {
        public static readonly List<ProductDto> _product = new List<ProductDto>
        {
            new ProductDto{ID = new Guid(),
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                StockLvl = 100,
                ResellPrice = 120.50,
            },
            new ProductDto {ID = new Guid(),
                ProductID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                StockLvl = 200,
                ResellPrice = 540.50,
            },
            new ProductDto {ID = new Guid(),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                StockLvl = 300,
                ResellPrice = 130.99,
            }
        };

        public static readonly List<ResellPrice> _resellPrice = new List<ResellPrice>
        {
            new ResellPrice {
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                ResellPrices = 19.99,
            },
            new ResellPrice {
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrices = 29.99,
            },
            new ResellPrice {
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                ResellPrices = 39.99,
            }
        };

        public static readonly List<ResellHistory> _resellHistory = new List<ResellHistory>
        {
            new ResellHistory {ID = new Guid(),
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                ResellPrice = 1000.50,
                DateTime = new DateTime(2020, 11, 01),
            },
            new ResellHistory {ID = new Guid(),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 500.50,
                DateTime = new DateTime(2020, 11, 08),
            },
            new ResellHistory {ID = new Guid(),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 100.50,
                DateTime = new DateTime(2020, 09, 24),
            },
        };

        public static ProductDto GetProductByProductID(Guid productID)
        {
            var product = _product.Where(s => s.ProductID == productID).FirstOrDefault();
            return product;
        }

        public static List<ProductDto> GetStockLevelOfProduct(int stockLevel)
        {
            var product = _product.Where(s => s.StockLvl <= stockLevel).ToList();
            return product;
        }

        public static ResellPrice GetResellPriceOfProducts(Guid productID)
        {
            var resellPrice= _resellPrice.Where(r => r.ProductID == productID).FirstOrDefault();
            return resellPrice;
        }

        public static List<ResellHistory> GetResellHistory(Guid productID)
        {
            var resellHistory = _resellHistory.Where(r => r.ProductID == productID).ToList();
            return resellHistory;
        }

        public static ProductDto SetResellPrice(Guid productID, double resellPrice)
        {
            var product = _product.Where(s => s.ProductID == productID).FirstOrDefault();
            if (product != null && resellPrice >= 0)
            {
                product.ResellPrice = resellPrice;
            }
            return product;
        }

        public static ProductDto SetStockLevelOfProduct(Guid productID, int stockLevel)
        {
            var product = _product.Where(s => s.ProductID == productID).FirstOrDefault();
            if (product != null && stockLevel >=0)
            {
                product.StockLvl = stockLevel;
            }
            return product;
        }
    }
}

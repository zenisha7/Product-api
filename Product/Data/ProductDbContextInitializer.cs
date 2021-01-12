using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Data
{
    public static class ProductDbContextInitializer
    {
        public static async Task TestData(ProductDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Products.Any() && context.ResellHistories.Any())
            {
                return;
            }
            var getProduct = new List<Models.ProductDto>
            {
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 11, ResellPrice = 11.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 12, ResellPrice = 12.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 13, ResellPrice = 13.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 14, ResellPrice = 14.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 15, ResellPrice = 15.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 16, ResellPrice = 16.99},
                new Models.ProductDto{ProductID = Guid.NewGuid(), StockLvl = 17, ResellPrice = 17.99},
            };
            getProduct.ForEach(g => context.Products.Add(g));
            var getResellHistory = new List<Models.ResellHistory>
            {
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 21.99, DateTime = new DateTime(2020, 12, 21)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 22.99, DateTime = new DateTime(2020, 12, 20)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 23.99, DateTime = new DateTime(2020, 12, 19)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 24.99, DateTime = new DateTime(2020, 12, 28)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 25.99, DateTime = new DateTime(2020, 12, 17)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 26.99, DateTime = new DateTime(2020, 12, 16)},
                new Models.ResellHistory{ProductID = getProduct[1].ProductID, ResellPrice = 27.99, DateTime = new DateTime(2020, 12, 12)},
            };
            getResellHistory.ForEach(c => context.ResellHistories.Add(c));
            await context.SaveChangesAsync();
        }
    }
}

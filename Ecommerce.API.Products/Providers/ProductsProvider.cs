using AutoMapper;
using Ecommerce.API.Products.DB;
using Ecommerce.API.Products.Interfaces;
using Ecommerce.API.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }       

        

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, 
            string ErrorMessage)>GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if (products != null && products.Any()) 
                {
                    var result = mapper.Map<IEnumerable<DB.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }            
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = mapper.Map<DB.Product, Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }




        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new DB.Product()
                {
                    Id = 1, Name = "Keyboard", Price = 20, Inventory = 100
                });
                dbContext.Products.Add(new DB.Product()
                {
                    Id = 2, Name = "Mouse", Price = 40, Inventory = 52
                });
                dbContext.Products.Add(new DB.Product()
                {
                   Id = 3, Name = "Monitor", Price = 150, Inventory = 61
                });
                dbContext.Products.Add(new DB.Product()
                {
                   Id = 4, Name = "CPU", Price = 5, Inventory = 74
                });
                dbContext.SaveChanges();
            }
        }     
    }
}

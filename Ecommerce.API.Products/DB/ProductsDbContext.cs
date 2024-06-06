using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Products.DB
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductsDbContext(DbContextOptions options):base(options)
        {
                
        }
    }
}

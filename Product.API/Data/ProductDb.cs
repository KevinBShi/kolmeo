using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Data
{
    public class ProductDb : DbContext
    {
        public ProductDb(DbContextOptions<ProductDb> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
    }
}

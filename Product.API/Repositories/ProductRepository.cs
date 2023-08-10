using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ProductDb _db;

        public ProductRepository(ProductDb db)
        {
            _db = db;
        }

        public async Task<IList<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task CreateAsync(Product item)
        {
            _db.Products.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Product item)
        {
            var exists = await GetByIdAsync(id);

            //this is only for updating
            //before updating, system should call GetByIdAsync to check existence

            exists.Name = item.Name;
            exists.Description = item.Description;
            exists.Price = item.Price;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await GetByIdAsync(id);

            //this is only for deleting
            //before deleting, system should call GetByIdAsync to check existence

            _db.Remove(exists);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Product>> MostExpensiveProductsAsync(int limit = 5)
        {
            return await _db.Products.OrderByDescending(p => p.Price)
                .Take(limit).ToListAsync();
        }
    }
}

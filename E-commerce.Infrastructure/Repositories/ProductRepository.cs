using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;
using E_commerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategory(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByFiltersAsync(ProductFilter productFilter)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (productFilter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productFilter.CategoryId.Value);
            }
            if (productFilter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= productFilter.MinPrice.Value);

            if (productFilter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= productFilter.MaxPrice.Value);
            if (productFilter.InStockOnly)
                query = query.Where(p => p.Stock > 0);
            if (!string.IsNullOrWhiteSpace(productFilter.SearchTerm))
            {
                var term = productFilter.SearchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term));
            }
            return await query.ToListAsync();
        }

        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .Include(p => p.Category)
                .FirstOrDefaultAsync()
                ;

            return product;
        }

        public Task<IEnumerable<Product>> GetInStockAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return await
                _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);
        }
    }
}

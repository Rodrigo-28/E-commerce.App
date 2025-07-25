using E_commerce.Domain.Models;

namespace E_commerce.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<bool> Delete(Product product);
        Task<bool> ExistsAsync(int productId);

        Task<Product> GetById(int productId);
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetByFiltersAsync(ProductFilter productFilter);

        Task<IEnumerable<Product>> GetByCategory(int categoryId);
        Task<IEnumerable<Product>> GetInStockAsync();

    }
}

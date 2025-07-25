using E_commerce.Domain.Models;
using System.Linq.Expressions;

namespace E_commerce.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> create(Category category);
        Task<Category> update(Category category);
        Task<bool> delete(Category category);
        Task<Category> GetOne(int categoryId);
        Task<IEnumerable<Category>> GetAll();
        Task<Category?> GetOne(Expression<Func<Category, bool>> predicate);
    }
}

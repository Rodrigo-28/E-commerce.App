using E_commerce.Domain.Models;
using System.Linq.Expressions;

namespace E_commerce.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAll();

        public Task<User?> GetOne(int userId);

        public Task<User> GetOne(Expression<Func<User, bool>> predicate);

        public Task<User> Create(User user);
        public Task<User> Update(User user);
    }
}

using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.Domain.Common;
using E_commerce.Domain.Models;
using System.Linq.Expressions;

namespace E_commerce.application.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponseDto?> GetOne(int userId);
        public Task<User> GetOne(Expression<Func<User, bool>> predicate);

        Task<IEnumerable<UserResponseDto>> GetAll();

        Task<GenericResult<UserResponseDto>> Create(CreateUserDto userDto);

        Task<UserResponseDto> Update(int userId, UpdateUserDto userDto);

        Task<bool> Delete(int userId);
    }
}

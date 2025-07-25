using E_commerce.Domain.Models;

namespace E_commerce.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(User user);
    }
}

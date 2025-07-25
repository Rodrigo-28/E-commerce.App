using E_commerce.application.Dtos.Requests;

namespace E_commerce.application.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponseDto> Login(LoginRequestDto userDto);

    }
}

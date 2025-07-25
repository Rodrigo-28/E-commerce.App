using E_commerce.application.Dtos.Requests;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace E_commerce.application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IConfiguration configuration, IUserService userService, IPasswordEncryptionService passwordEncryptionService, IJwtTokenService jwtTokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _passwordEncryptionService = passwordEncryptionService;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto userDto)
        {
            var user = await _userService.GetOne(u => u.Email == userDto.Email);

            if (user == null)
            {
                //Todo - Invalid credentials (403)

                throw new BadRequestException("Invalid email")
                {
                    ErrorCode = "001"
                };
            }

            //Check password

            var isValid = _passwordEncryptionService.VerifyPassword(user.Password, userDto.Password);

            if (!isValid)
            {
                //Todo - Invalid Password (403)
                throw new BadRequestException("Invalid password")
                {
                    ErrorCode = "002"
                };
            }

            //Generate token
            string token = _jwtTokenService.GenerateJwtToken(user);

            return new LoginResponseDto() { Token = token };
        }
    }
}

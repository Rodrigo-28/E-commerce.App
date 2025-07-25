using E_commerce.application.Dtos.Requests;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto, IValidator<LoginRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString())
                {
                    ErrorCode = "004"
                };
            }
            var res = await _authService.Login(loginDto);

            if (res == null) return NotFound();

            return Ok(res);
        }
    }
}

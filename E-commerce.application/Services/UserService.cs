using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Common;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;
using System.Linq.Expressions;

namespace E_commerce.application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;

        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IPasswordEncryptionService passwordEncryptionService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordEncryptionService = passwordEncryptionService;
            _mapper = mapper;

        }
        public async Task<GenericResult<UserResponseDto>> Create(CreateUserDto userDto)
        {
            var newUser = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password, //PLain text
                RoleId = userDto.RoleId
            };



            //if (!validation.Success)
            //{
            //    return GenericResult<UserResponseDto>.FailureResult(validation.Messages);
            //}

            //Hash and store password
            newUser.Password = _passwordEncryptionService.HashPassword(userDto.Password);

            var user = await _userRepository.Create(newUser);

            var data = _mapper.Map<UserResponseDto>(user);

            return GenericResult<UserResponseDto>.SuccessResult(data);
        }

        public Task<bool> Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserResponseDto>> GetAll()
        {
            var users = await _userRepository.GetAll();

            if (!users.Any())
            {
                throw new UnauthorizedException("Unauthorized")
                {
                    ErrorCode = "004"
                };
            }

            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetOne(int userId)
        {
            var user = await _userRepository.GetOne(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found")
                {
                    ErrorCode = "003"
                };
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<User> GetOne(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.GetOne(predicate);
        }

        public async Task<UserResponseDto> Update(int userId, UpdateUserDto userDto)
        {
            var currentUser = await _userRepository.GetOne(userId);

            if (currentUser == null)
            {
                throw new BadRequestException($"No user found with id {userId}")
                {
                    ErrorCode = "005"
                };

            }
            currentUser.Username = userDto.Username;
            currentUser.Password = _passwordEncryptionService.HashPassword(userDto.Password);
            currentUser.RoleId = userDto.RoleId;


            var updatedUser = await _userRepository.Update(currentUser);

            return _mapper.Map<UserResponseDto>(updatedUser);
        }
    }
}

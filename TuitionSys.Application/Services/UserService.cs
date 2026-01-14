using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;
using TuitionSys.Domain.Interfaces;
using TuitionSys.Infrastructure;

namespace TuitionSys.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role,
            });
        }
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };
        }
        public async Task AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
            };
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.UserId);
            if (user == null) return;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return;
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
        public async Task<UserDto> AuthenticateUserAsync(string email, string password)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email && u.Password == password);
            var user = users.FirstOrDefault();
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            return !users.Any();

        }
    }
}

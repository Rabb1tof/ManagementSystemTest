using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;
using ProjectManagementSystem.Console.Repositories;

namespace ProjectManagementSystem.Console.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            var passwordHash = HashPassword(password);
            return user.PasswordHash == passwordHash ? user : null;
        }

        public async Task<User> RegisterAsync(string username, string password, UserRole role)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = role
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (user.PasswordHash != HashPassword(oldPassword))
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 
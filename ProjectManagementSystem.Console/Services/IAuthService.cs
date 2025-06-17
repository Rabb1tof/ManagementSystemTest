using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password, UserRole role);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
} 
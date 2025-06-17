using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
} 
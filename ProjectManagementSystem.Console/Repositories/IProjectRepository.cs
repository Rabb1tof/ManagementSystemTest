using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> CreateAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
    }
} 
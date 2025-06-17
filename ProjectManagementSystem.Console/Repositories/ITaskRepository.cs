using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Repositories
{
    public interface ITaskRepository
    {
        Task<ProjectTask> GetByIdAsync(int id);
        Task<IEnumerable<ProjectTask>> GetAllAsync();
        Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<ProjectTask>> GetByUserIdAsync(int userId);
        Task<ProjectTask> CreateAsync(ProjectTask task);
        Task<ProjectTask> UpdateAsync(ProjectTask task);
        Task<bool> DeleteAsync(int id);
    }
} 
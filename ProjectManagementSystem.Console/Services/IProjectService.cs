using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Services
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(string name, string description);
        Task<Project> GetProjectAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
    }
} 
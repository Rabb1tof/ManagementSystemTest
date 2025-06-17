using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Services
{
    public interface ITaskService
    {
        Task<ProjectTask> CreateTaskAsync(int projectId, string title, string description, int assignedToUserId);
        Task<ProjectTask> GetTaskAsync(int id);
        Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId);
        Task<IEnumerable<ProjectTask>> GetTasksByUserAsync(int userId);
        Task<bool> UpdateTaskStatusAsync(int taskId, ProjectTaskStatus newStatus);
        Task<bool> UpdateTaskAsync(ProjectTask task);
        Task<bool> DeleteTaskAsync(int id);
    }
} 
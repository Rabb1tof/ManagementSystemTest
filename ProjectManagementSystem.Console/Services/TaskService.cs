using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;
using ProjectManagementSystem.Console.Repositories;

namespace ProjectManagementSystem.Console.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<ProjectTask> CreateTaskAsync(int projectId, string title, string description, int assignedToUserId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Проект с ID {projectId} не найден");
            }

            var user = await _userRepository.GetByIdAsync(assignedToUserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Пользователь с ID {assignedToUserId} не найден");
            }

            var task = new ProjectTask
            {
                ProjectId = projectId,
                Title = title,
                Description = description,
                AssignedToUserId = assignedToUserId
            };

            return await _taskRepository.CreateAsync(task);
        }

        public async Task<ProjectTask> GetTaskAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId)
        {
            return await _taskRepository.GetByProjectIdAsync(projectId);
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByUserAsync(int userId)
        {
            return await _taskRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, ProjectTaskStatus newStatus)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return false;
            }

            task.Status = newStatus;
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
            return true;
        }

        public async Task<bool> UpdateTaskAsync(ProjectTask task)
        {
            var existingTask = await _taskRepository.GetByIdAsync(task.Id);
            if (existingTask == null)
            {
                return false;
            }

            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
} 
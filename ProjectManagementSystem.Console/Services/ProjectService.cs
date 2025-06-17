using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;
using ProjectManagementSystem.Console.Repositories;

namespace ProjectManagementSystem.Console.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateProjectAsync(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название проекта не может быть пустым", nameof(name));
            }

            var project = new Project
            {
                Name = name,
                Description = description
            };

            return await _projectRepository.CreateAsync(project);
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException("Название проекта не может быть пустым", nameof(project));
            }

            try
            {
                await _projectRepository.UpdateAsync(project);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            return await _projectRepository.DeleteAsync(id);
        }
    }
} 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Repositories
{
    public class JsonProjectRepository : IProjectRepository
    {
        private readonly string _filePath;
        private List<Project> _projects;

        public JsonProjectRepository(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _projects = JsonSerializer.Deserialize<List<Project>>(json) ?? new List<Project>();
            }
            else
            {
                _projects = new List<Project>();
                SaveData();
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_projects, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await Task.FromResult(_projects.FirstOrDefault(p => p.Id == id));
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await Task.FromResult(_projects);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            project.Id = _projects.Count > 0 ? _projects.Max(p => p.Id) + 1 : 1;
            _projects.Add(project);
            SaveData();
            return await Task.FromResult(project);
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException($"Проект с ID {project.Id} не найден");
            }

            var index = _projects.IndexOf(existingProject);
            _projects[index] = project;
            SaveData();
            return await Task.FromResult(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return await Task.FromResult(false);
            }

            _projects.Remove(project);
            SaveData();
            return await Task.FromResult(true);
        }
    }
} 
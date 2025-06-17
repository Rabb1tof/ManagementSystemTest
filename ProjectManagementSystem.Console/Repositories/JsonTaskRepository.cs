using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;

namespace ProjectManagementSystem.Console.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        private List<ProjectTask> _tasks;

        public JsonTaskRepository(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _tasks = JsonSerializer.Deserialize<List<ProjectTask>>(json) ?? new List<ProjectTask>();
            }
            else
            {
                _tasks = new List<ProjectTask>();
                SaveData();
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task<ProjectTask> GetByIdAsync(int id)
        {
            return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            return await Task.FromResult(_tasks);
        }

        public async Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(int projectId)
        {
            return await Task.FromResult(_tasks.Where(t => t.ProjectId == projectId));
        }

        public async Task<IEnumerable<ProjectTask>> GetByUserIdAsync(int userId)
        {
            return await Task.FromResult(_tasks.Where(t => t.AssignedToUserId == userId));
        }

        public async Task<ProjectTask> CreateAsync(ProjectTask task)
        {
            task.Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
            _tasks.Add(task);
            SaveData();
            return await Task.FromResult(task);
        }

        public async Task<ProjectTask> UpdateAsync(ProjectTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Задача с ID {task.Id} не найдена");
            }

            var index = _tasks.IndexOf(existingTask);
            _tasks[index] = task;
            SaveData();
            return await Task.FromResult(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return await Task.FromResult(false);
            }

            _tasks.Remove(task);
            SaveData();
            return await Task.FromResult(true);
        }
    }
} 
using System;
using System.Collections.Generic;

namespace ProjectManagementSystem.Console.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Task> Tasks { get; set; }

        public Project()
        {
            CreatedAt = DateTime.UtcNow;
            Tasks = new List<Task>();
        }
    }
} 
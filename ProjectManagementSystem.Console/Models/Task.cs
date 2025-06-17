using System;

namespace ProjectManagementSystem.Console.Models
{
    public enum ProjectTaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public class ProjectTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public int AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ProjectTask()
        {
            CreatedAt = DateTime.UtcNow;
            Status = ProjectTaskStatus.ToDo;
        }
    }
} 
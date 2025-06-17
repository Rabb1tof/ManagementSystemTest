using System;

namespace ProjectManagementSystem.Console.Models
{
    public enum UserRole
    {
        Employee,
        Manager
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
} 
using System;

namespace TaskManagementApi.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }  // Navigation property
        public string Priority { get; set; } = string.Empty; // "Low", "Medium", "High"
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
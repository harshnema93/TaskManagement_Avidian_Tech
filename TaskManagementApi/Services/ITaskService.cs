using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        Task<List<TaskManagementApi.Models.Task>> GetTasksAsync(string? searchTerm, string? category, string? priority);
        Task<TaskManagementApi.Models.Task?> GetTaskByIdAsync(int id);
        Task<TaskManagementApi.Models.Task> CreateTaskAsync(TaskManagementApi.Models.Task task);
        Task<TaskManagementApi.Models.Task?> UpdateTaskAsync(int id, TaskManagementApi.Models.Task task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
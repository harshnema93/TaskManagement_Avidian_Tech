using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(TaskDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskManagementApi.Models.Task>> GetTasksAsync(string? searchTerm, string? category, string? priority)
        {
            try
            {
                _logger.LogInformation("Fetching tasks with filter: search={Search}, category={Category}, priority={Priority}", searchTerm, category, priority);

                // Build the base query using IQueryable for efficient filtering
                IQueryable<TaskManagementApi.Models.Task> query = _context.Tasks.Include(t => t.Category);

                // Apply filters only if the parameters are provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(t => t.Title.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm)));
                }
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(t => t.Category != null && t.Category.Name == category);
                }
                if (!string.IsNullOrEmpty(priority))
                {
                    query = query.Where(t => t.Priority == priority);
                }

                // Execute the query only once, at the end.
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks.");
                throw;
            }
        }
        
        public async Task<TaskManagementApi.Models.Task?> GetTaskByIdAsync(int id)
        {
            try
            {
                 _logger.LogInformation("Fetching task with ID: {TaskId}", id);
                return await _context.Tasks.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex) {
                 _logger.LogError(ex, "Error fetching task with ID: {TaskId}", id);
                throw;
            }
           
        }

        public async Task<TaskManagementApi.Models.Task> CreateTaskAsync(TaskManagementApi.Models.Task task)
        {
            try
            {
                 _logger.LogInformation("Creating new task: {TaskTitle}", task.Title);
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                 _logger.LogInformation("Created task with ID: {TaskId}", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task: {TaskTitle}", task.Title);
                throw;
            }
        }

        public async Task<TaskManagementApi.Models.Task?> UpdateTaskAsync(int id, TaskManagementApi.Models.Task task)
        {
           try
           {
                _logger.LogInformation("Updating task with ID: {TaskId}", id);
            var existingTask = await _context.Tasks.FindAsync(id);
                if (existingTask == null)
                {
                    _logger.LogWarning("Task with ID: {TaskId} not found for update.", id);
                    return null;
                }
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.CategoryId = task.CategoryId;
                existingTask.Priority = task.Priority;
                existingTask.DueDate = task.DueDate;
                existingTask.IsCompleted = task.IsCompleted;


                await _context.SaveChangesAsync();
                 _logger.LogInformation("Updated task with ID: {TaskId}", id);
                return existingTask;
           }
           catch (Exception ex)
           {
                _logger.LogError(ex, "Error updating task with ID: {TaskId}", id);
                throw;
            }

        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                     _logger.LogWarning("Task with ID: {TaskId} not found for deletion.", id);
                    return false;
                }
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted task with ID: {TaskId}", id);
                return true;
            }
            catch(Exception ex)
            {
                 _logger.LogError(ex, "Error deleting task with ID: {TaskId}", id);
                throw;
            }
        }
    }
}
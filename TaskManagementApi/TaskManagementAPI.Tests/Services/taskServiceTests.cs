using System;
using System.Collections.Generic;
using System.Threading.Tasks; // For asynchronous operations
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Xunit;

// Alias to avoid ambiguity
using TaskModel = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly TaskDbContext _context;
        private readonly Mock<ILogger<TaskService>> _loggerMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskTestDB")
                .Options;

            _context = new TaskDbContext(options);
            _loggerMock = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsAllTasks_WhenNoFilters()
        {
            // Arrange
            await _context.Tasks.AddRangeAsync(
                new TaskModel { Title = "Task 1", Priority = "High" },
                new TaskModel { Title = "Task 2", Priority = "Low" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetTasksAsync(null, null, null);

            // Assert
            Assert.Equal(2, result.Count);
            }


        [Fact]
        public async Task GetTasksAsync_ReturnsFilteredTasks_WhenSearchTermIsProvided()
        {
            // Arrange
            await _context.Tasks.AddRangeAsync(
                new TaskModel { Title = "Task 1", Description = "Test 1", Priority = "High" },
                new TaskModel { Title = "Task 2", Description = "Test 2", Priority = "Low" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetTasksAsync("Test 1", null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Task 1", result[0].Title);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsFilteredTasks_WhenCategoryIsProvided()
        {
            // Arrange
            var category1 = new Category { Name = "Category 1" };
            var category2 = new Category { Name = "Category 2" };
            await _context.Categories.AddRangeAsync(category1, category2);
            await _context.SaveChangesAsync();

            await _context.Tasks.AddRangeAsync(
                new TaskModel { Title = "Task 1", Category = category1, Priority = "High" },
                new TaskModel { Title = "Task 2", Category = category2, Priority = "Low" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetTasksAsync(null, "Category 1", null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Task 1", result[0].Title);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsFilteredTasks_WhenPriorityIsProvided()
        {
            // Arrange
            await _context.Tasks.AddRangeAsync(
                new TaskModel { Title = "Task 1", Priority = "High" },
                new TaskModel { Title = "Task 2", Priority = "Low" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetTasksAsync(null, null, "High");

            // Assert
            Assert.Single(result);
            Assert.Equal("Task 1", result[0].Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenTaskExists()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Title = "Test Task" };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsNull_WhenTaskNotExists()
        {
            // Act
            var result = await _taskService.GetTaskByIdAsync(10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateTaskAsync_CreatesTask_ReturnsCreatedTask()
        {
            // Arrange
            var newTask = new TaskModel { Title = "New Task", Priority = "Medium" };

            // Act
            var result = await _taskService.CreateTaskAsync(newTask);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Task", result.Title);
            Assert.Equal(1, _context.Tasks.CountAsync().Result);
        }

        [Fact]
        public async Task UpdateTaskAsync_ReturnsUpdatedTask()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Title = "Original Task", Priority = "Medium" };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var updatedTask = new TaskModel { Id = 1, Title = "Updated Task", Priority = "High" };

            // Act
            var result = await _taskService.UpdateTaskAsync(1, updatedTask);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Title);
            Assert.Equal("High", result.Priority);
        }

        [Fact]
        public async Task UpdateTaskAsync_ReturnsNull_WhenTaskNotFound()
        {
            // Arrange
            var updatedTask = new TaskModel { Id = 1, Title = "Updated Task", Priority = "High" };

            // Act
            var result = await _taskService.UpdateTaskAsync(1, updatedTask);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_DeletesTask_ReturnsTrue()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Title = "Original Task" };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.DeleteTaskAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, _context.Tasks.CountAsync().Result);
        }

        [Fact]
        public async Task DeleteTaskAsync_ReturnsFalse_WhenTaskNotFound()
        {
            // Act
            var result = await _taskService.DeleteTaskAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}

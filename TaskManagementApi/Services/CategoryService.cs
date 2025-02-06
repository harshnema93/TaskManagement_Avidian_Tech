using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TaskDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(TaskDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching categories.");
                return await _context.Categories.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories.");
                throw;
            }

        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                 _logger.LogInformation("Creating new category: {CategoryName}", category.Name);
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created category with ID: {CategoryId}", category.Id);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryName}", category.Name);
                throw;
            }
        }

    }
}
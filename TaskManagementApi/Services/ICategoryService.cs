using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
    }
}
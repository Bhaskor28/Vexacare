using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Categories;
using Vexacare.Application.Locations;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryVM>> GetAllCategories()
        {
            var categories = await _context.Categories
                .OrderBy(l => l.Name)
                .ToListAsync();

            return categories.Select(location => new CategoryVM
            {
                Id = location.Id,
                Name = location.Name
            });
        }

        public Task<CategoryVM> GetCategoriesByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}

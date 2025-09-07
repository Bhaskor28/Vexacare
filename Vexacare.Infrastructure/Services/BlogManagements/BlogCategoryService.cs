using Microsoft.EntityFrameworkCore;
using Vexacare.Application.BlogPosts;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.BlogManagements
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly ApplicationDbContext _context;

        public BlogCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BlogCategoryVM>> GetAllcategorysAsync()
        {
            var categories = await _context.BlogCategories
                .OrderBy(l => l.Name)
                .ToListAsync();

            return categories.Select(categories => new BlogCategoryVM
            {
                Id = categories.Id,
                Name = categories.Name
            });
        }

        public async Task<BlogCategoryVM> GetcategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(l => l.Id == id);

            if (category == null)
            {
                return null; // Or throw an exception if preferred
            }

            return new BlogCategoryVM
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}

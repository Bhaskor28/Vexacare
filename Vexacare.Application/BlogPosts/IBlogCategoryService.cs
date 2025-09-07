using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Locations;

namespace Vexacare.Application.BlogPosts
{
    public interface IBlogCategoryService
    {
        Task<IEnumerable<BlogCategoryVM>> GetAllcategorysAsync(); // Changed to plural
        Task<BlogCategoryVM> GetcategoryByIdAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.DoctorProfiles;

namespace Vexacare.Application.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllCategories();
        Task<CategoryVM> GetCategoriesByIdAsync(int categoryId);
    }
}

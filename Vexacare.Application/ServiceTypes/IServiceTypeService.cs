using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Categories;

namespace Vexacare.Application.ServiceTypes
{
    public interface IServiceTypeService
    {
        Task<IEnumerable<ServiceTypeVM>> GetAllServiceTypes();
        Task<ServiceTypeVM> GetCategoriesByIdAsync(int serviceTypeId);
    }
}

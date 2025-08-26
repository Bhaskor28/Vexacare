using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Categories;
using Vexacare.Application.ServiceTypes;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.ServiceTypes
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ServiceTypeVM>> GetAllServiceTypes()
        {
            var serviceTypes = await _context.ServiceTypes
                .OrderBy(l => l.Name)
                .ToListAsync();

            return serviceTypes.Select(location => new ServiceTypeVM
            {
                Id = location.Id,
                Name = location.Name
            });
        }

        public Task<ServiceTypeVM> GetCategoriesByIdAsync(int serviceTypeId)
        {
            throw new NotImplementedException();
        }
    }
}

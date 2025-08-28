using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Locations;
using Vexacare.Application.UsersVM;

namespace Vexacare.Application.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationVM>> GetAllLocationsAsync(); // Changed to plural
        Task<LocationVM> GetLocationByIdAsync(int id);
    }
}

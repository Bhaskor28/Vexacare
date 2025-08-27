using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Locations;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.LocationServices
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocationVM>> GetAllLocationsAsync()
        {
            var locations = await _context.Locations
                .OrderBy(l => l.Name)
                .ToListAsync();

            return locations.Select(location => new LocationVM
            {
                Id = location.Id,
                Name = location.Name
            });

        }
        public async Task<LocationVM> GetLocationByIdAsync(int id)
        {
            var location = await _context.Locations
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return null; // Or throw an exception if preferred
            }

            return new LocationVM
            {
                Id = location.Id,
                Name = location.Name
            };
        }
    }
}

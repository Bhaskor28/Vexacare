using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.PatientDashboardServices
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly ApplicationDbContext _context;

        public PatientDashboardService(
            ApplicationDbContext context

            )
        {
            _context = context;
        }

        public async Task<DashboardVM> GetPatientKitOrderByIdAsync(string userId)
        {
            var x = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new DashboardVM
                {
                    Id = userId,
                    KitState = o.KitState,
                    StateStatus = o.StateStatus
                })
                .FirstOrDefaultAsync() ?? new DashboardVM { Id = userId, KitState = null, StateStatus = null };
            return x;
        }
    }
}

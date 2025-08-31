using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Patients.ViewModels;

namespace Vexacare.Application.Interfaces
{
    public interface IPatientDashboardService
    {
       Task<DashboardVM> GetPatientKitOrderByIdAsync(string userId);
    }
}

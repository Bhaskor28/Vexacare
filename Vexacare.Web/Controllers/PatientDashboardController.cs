using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Services.PatientDashboardServices;

namespace Vexacare.Web.Controllers
{
    public class PatientDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientDashboardService _patientDashboardService;

        public PatientDashboardController(
            UserManager<ApplicationUser> userManager,
            IPatientDashboardService patientDashboardService
            )
        {
            _userManager = userManager;
            _patientDashboardService = patientDashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) {
                return RedirectToAction("Login", "Account");
            }
            var dashboardVM = await _patientDashboardService.GetPatientKitOrderByIdAsync(currentUser.Id);
            return View(dashboardVM);
        }
    }
}

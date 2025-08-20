using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Web.Models;

namespace Vexacare.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Patient> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Single constructor with all dependencies
        public HomeController(
            ILogger<HomeController> logger,
            UserManager<Patient> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

            // Add doctors to ViewBag
            ViewBag.Doctors = doctors;
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

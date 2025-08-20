using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vexacare.Application.Doctors.ViewModel;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        #region Constructor
        public AdminController(
            UserManager<Patient> userManager,
            SignInManager<Patient> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _roleManager = roleManager;
        }
        #endregion

        #region Doctor List
        [HttpGet]
        public async Task<IActionResult> DoctorList()
        {
            // Check if Doctor role exists, if not create it
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            // Get all users with the Doctor role
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

            // Map to view model if needed, or pass the Patient entities directly
            return View(doctors);
        }
        #endregion
        #region register
        [HttpGet]
        public IActionResult RegisterDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new Patient
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign Patient role
                    await _userManager.AddToRoleAsync(user, "Doctor");
                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("DoctorList", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        
    }
}

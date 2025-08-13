using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vexacare.Application.Patients.ViewModels;
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


        #region Constructor
        public AdminController(
        UserManager<Patient> userManager,
        SignInManager<Patient> signInManager,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        #endregion
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(AdminVM model)
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
                    //await _userManager.AddToRoleAsync(user, "Patient");
                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVM model)
        {
            if (ModelState.IsValid)
            {
                // Find user by email (since we're using email as username)
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Attempt to sign in
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        model.RememberMe,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Redirect to returnUrl if provided, otherwise to home
                        return RedirectToAction("Index", "Admin");
                    }
                }

                // If we got this far, something failed
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }
    }
}

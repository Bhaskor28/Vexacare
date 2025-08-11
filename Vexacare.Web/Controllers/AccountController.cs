using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
        UserManager<Patient> userManager,
        SignInManager<Patient> signInManager,
        ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
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
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        //step 1: basic info

        [HttpGet]
        public IActionResult BasicInfo()
        {
            return View();
        }
        //end of step 1
        //step 2: Health info

        [HttpGet]
        public IActionResult HealthInfo()
        {
            return View();
        }
        //end of step 2

        //step 3: Gastrointestinal info

        [HttpGet]
        public IActionResult GastrointestinalInfo()
        {
            return View();
        }
        //end of step 3

        //step 4: Symtoms info

        [HttpGet]
        public IActionResult SymtomsInfo()
        {
            return View();
        }
        //end of step 4

        //step 5: DietProfile info

        [HttpGet]
        public IActionResult DietProfileInfo()
        {
            return View();
        }
        //end of step 5

        //step 6: Lifestyle info

        [HttpGet]
        public IActionResult LifestyleInfo()
        {
            return View();
        }
        //end of step 6

        //step 7: Symtoms info

        [HttpGet]
        public IActionResult TherapiesInfo()
        {
            return View();
        }
        //end of step 7

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
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
                        return RedirectToAction("Index", "Home");
                    }
                }

                // If we got this far, something failed
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

    }
}

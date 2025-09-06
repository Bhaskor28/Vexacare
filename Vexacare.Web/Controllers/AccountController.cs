using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Application.Users.Patients;
using Vexacare.Domain.Entities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICartService _cartService;
        private readonly IPatientService _patientService;
        #endregion

        #region Constructor
        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        ICartService cartService,
        IPatientService patientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _cartService = cartService;
            _patientService = patientService;
        }
        #endregion

        #region register
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
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                try
                {

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Assign Patient role
                        await _userManager.AddToRoleAsync(user, "Patient");
                        return RedirectToAction("Login", "Account");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the account. Please try again.");
                }

            }

            return View(model);
        }
        #endregion
        
        #region BasicInfo
        //step 1: basic info
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BasicInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var model = await _patientService.GetBasicInfoAsync(patientId) ?? new BasicInfoVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BasicInfo(BasicInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);
                var result = await _patientService.SaveBasicInfoAsync(patientId, model);
                if (result)
                {
                    return RedirectToAction("HealthInfo", "Account");
                }
            }
            return View("BasicInfo", model);
        }

        //end of step 1
        #endregion

        #region HealthInfo
        //step 2: Health info
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> HealthInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var model = await _patientService.GetHealthInfoAsync(patientId) ?? new HealthInfoVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HealthInfo(HealthInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);
                var result = await _patientService.SaveHealthInfoAsync(patientId, model);
                if (result)
                {
                    return RedirectToAction("GastrointestinalInfo", "Account");
                }
            }
            return View("HealthInfo", model);
        }
        //end of step 2
        #endregion

        #region GastrointestinalInfo
        //step 3: Gastrointestinal info
        public async Task<IActionResult> GastrointestinalInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var model = await _patientService.GetGastrointestinalInfoAsync(patientId) ?? new GastrointestinalInfoVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GastrointestinalInfo(GastrointestinalInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _patientService.SaveGastrointestinalInfoAsync(userId, model);
                if (result)
                {
                    return RedirectToAction("SymptomsInfo", "Account");
                }
                return RedirectToAction("SymptomsInfo", "Account");
            }
            return View("GastrointestinalInfo", model);
        }
        //end of step 3
        #endregion

        #region SymptomsInfo
        //step 4: Symtoms info
        [HttpGet]
        public async Task<IActionResult> SymptomsInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var model = await _patientService.GetSymptomsInfoAsync(patientId) ?? new SymptomsInfoVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SymptomsInfo(SymptomsInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);
                // Check if SymptomsInfo already exists for this patient
                var result = await _patientService.SaveSymptomsInfoAsync(patientId, model);
                if (result)
                {
                    return RedirectToAction("DietProfileInfo", "Account");
                }
            }
            return View("SymptomsInfo", model);
        }
        //end of step 4

        #endregion


        #region DietProfileInfo
        //step 5: DietProfile info
        public async Task<IActionResult> DietProfileInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var dietInfo = await _patientService.GetDietProfileInfoAsync(patientId) ?? new DietProfileInfoVM();
            return View(dietInfo);
        }

        [HttpPost]
        public async Task<IActionResult> DietProfileInfo(DietProfileInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);
                var retult = await _patientService.SaveDietProfileInfoAsync(patientId, model);
                if(retult)
                {
                    return RedirectToAction("LifestyleInfo", "Account");
                }
            }
            return View("DietProfileInfo", model);
        }
        //end of step 5
        #endregion

        #region LifestyleInfo
        //step 6: Lifestyle info
        [HttpGet]
        public async Task<IActionResult> LifestyleInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var lifestyleInfo = await _patientService.GetLifestyleInfoAsync(patientId) ?? new LifestyleInfoVM();
            return View(lifestyleInfo);
        }

        [HttpPost]
        public async Task<IActionResult> LifestyleInfo(LifestyleInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _patientService.SaveLifestyleInfoAsync(userId, model);
                if (result)
                {
                    return RedirectToAction("TherapiesInfo", "Account");
                }
            }
            return View("LifestyleInfo", model);
        }
        //end of step 6
        #endregion


        #region TherapiesInfo
        [HttpGet]
        public async Task<IActionResult> TherapiesInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var model = await _patientService.GetTherapiesInfoAsync(patientId) ?? new TherapiesInfoVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TherapiesInfo(TherapiesInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _patientService.SaveTherapiesInfoAsync(userId, model);
                if(result)
                {
                    return RedirectToAction("Index", "PatientDashboard");
                }
            }
            return View(model);
        }
        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }
        #endregion
        #region SignOut
        //Sign Out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var userId = _userManager.GetUserId(User);
            // Clear cart after successful order
            await _cartService.ClearCartAsync(userId);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}

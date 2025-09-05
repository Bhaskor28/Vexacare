using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vexacare.Application.Categories;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.ServiceTypes;
using Vexacare.Domain.Entities.DoctorEntities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;
using Vexacare.Infrastructure.Services;

namespace Vexacare.Web.Controllers
{
    public class DoctorsController : Controller
    {
        #region Fields
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IServiceTypeService _serviceTypeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        #region Constructor
        public DoctorsController(
            IDoctorProfileService doctorProfileService,
            ILocationService locationService,
            ICategoryService categoryService,
            IServiceTypeService serviceTypeService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
        {
            _doctorProfileService = doctorProfileService;
            _locationService = locationService;
            _categoryService = categoryService;
            _serviceTypeService = serviceTypeService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Locations = await _locationService.GetAllLocationsAsync();


            var doctorProfile = new ProfileBasicVM();
            var doctorSession = new DoctorSessionVM();

            var existingDoctorProfile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(currentUser.Id);
            var existingDoctorSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(currentUser.Id);

            if (existingDoctorProfile != null)
            {
                doctorProfile = existingDoctorProfile;
            }
            if (existingDoctorSession != null)
            {
                doctorSession = existingDoctorSession;
            }

            doctorProfile.Name = $"{currentUser.FirstName} {currentUser.LastName}";
            doctorProfile.Email = currentUser.Email;

            var partnerHubVM = new PartnerHubVM
            {
                ProfileBasic = doctorProfile,
                ProfileSession = doctorSession
            };



            //var viewModel = new ProfileBasicVM
            //{
            //    UserId = currentUser.Id,
            //    Name = $"{currentUser.FirstName} {currentUser.LastName}",
            //    Email = currentUser.Email,
            //    ServiceTypeId = doctorProfile?.ServiceTypeId,
            //    LocationId = doctorProfile?.LocationId,
            //    CategoryId = doctorProfile?.CategoryId,
            //    AreaofExperties = doctorProfile?.AreaofExperties,
            //    Gender = doctorProfile?.Gender,
            //    About = doctorProfile?.About,
            //    EducationDetails = doctorProfile?.EducationDetails,
            //    ProfileImagePath = doctorProfile?.ProfileImagePath
            //};
            //return View(viewModel);
            return View(partnerHubVM);
        }

        #region Edit Profile Basic
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfileBasic(PartnerHubVM model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //if (!ModelState.IsValid)
            //{
            //    ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            //    ViewBag.Categories = await _categoryService.GetAllCategories();
            //    ViewBag.Locations = await _locationService.GetAllLocationsAsync();
            //    return View(model);
            //}



            try
            {
                var existingProfile = await _context.DoctorProfiles
                .FirstOrDefaultAsync(d => d.UserId == currentUser.Id);

                // Set the ID from the existing profile to ensure update, not create
                if (existingProfile != null)
                {
                    model.ProfileBasic.Id = existingProfile.Id; // THIS IS THE CRITICAL LINE
                }
                
                model.ProfileBasic.UserId = currentUser.Id;
                await _doctorProfileService.CreateDoctorBasicProfile(model.ProfileBasic);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving: {ex.Message}");
                ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
                ViewBag.Categories = await _categoryService.GetAllCategories();
                ViewBag.Locations = await _locationService.GetAllLocationsAsync();
                return View(model);
            }
        }

        #endregion

        #region Save Availability
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAvailability(PartnerHubVM model)
        {
            if (!ModelState.IsValid)
            {
                // Return to view with errors
                var doctorProfileId = model.ProfileBasic.UserId;
                var profileBasic = await _doctorProfileService.GetDoctorProfileByUserIdAsync(doctorProfileId);

                return View("Index", model);
            }

            try
            {
                var success = await _doctorProfileService.SaveProfileSettingsAsync(model.ProfileSession);

                if (success)
                {
                    TempData["SuccessMessage"] = "Availability settings updated successfully!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to update availability settings.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            // If we got here, something went wrong
            var currentDoctorProfileId = model.ProfileBasic.UserId;
            var currentProfileBasic = await _doctorProfileService.GetDoctorProfileByUserIdAsync(currentDoctorProfileId);

            return View("Index", model);
        }

        #endregion

        #region change passwrod

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(PartnerHubVM model)
        {
            // Preserve the active tab
            if (!string.IsNullOrEmpty(Request.Form["ActiveTab"]))
            {
                ViewBag.ActiveTab = Request.Form["ActiveTab"];
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Only proceed with password change if ModelState is valid
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(
                        currentUser,
                        model.ChangePassword.OldPassword,
                        model.ChangePassword.NewPassword
                    );

                    if (result.Succeeded)
                    {
                        // Refresh the sign-in cookie to reflect the password change
                        await _signInManager.RefreshSignInAsync(currentUser);

                        TempData["SuccessMessage"] = "Password changed successfully!";
                        return RedirectToAction("Index");
                    }

                    //// Add errors from Identity to ModelState
                    //foreach (var error in result.Errors)
                    //{
                    //    ModelState.AddModelError("", error.Description);
                    //    // Add field-specific errors if available
                    //    if (error.Code.Contains("Password"))
                    //    {
                    //        ModelState.AddModelError("ChangePassword.NewPassword", error.Description);
                    //    }
                    //}
                    ModelState.AddModelError("ChangePassword.OldPassword", "Incorrect Password.");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while changing password: {ex.Message}");
            }

            // If we got here, something went wrong - reload all necessary data
            ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Locations = await _locationService.GetAllLocationsAsync();

            var profile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(currentUser.Id);
            var session = await _doctorProfileService.GetDoctorSessionByUserIdAsync(currentUser.Id);

            model.ProfileBasic = profile ?? new ProfileBasicVM();
            model.ProfileSession = session ?? new DoctorSessionVM();

            // Ensure the change password tab is active when returning with errors
            ViewBag.ActiveTab = "change-password";

            return View("Index", model);
        }

        #endregion




    }
}

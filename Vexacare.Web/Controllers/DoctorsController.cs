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
        //public async Task<IActionResult> EditProfileBasic()
        //{
        //    // Get the currently logged-in user
        //    var currentUser = await _userManager.GetUserAsync(User);

        //    if (currentUser == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
        //    ViewBag.Categories = await _categoryService.GetAllCategories();
        //    ViewBag.Locations = await _locationService.GetAllLocationsAsync();

        //    // Load existing doctor profile
        //    var doctorProfile = await _context.DoctorProfiles
        //        .FirstOrDefaultAsync(d => d.UserId == currentUser.Id);

        //    var viewModel = new ProfileBasicVM
        //    {
        //        UserId = currentUser.Id,
        //        Name = $"{currentUser.FirstName} {currentUser.LastName}",
        //        Email = currentUser.Email,
        //        ServiceTypeId = doctorProfile?.ServiceTypeId,
        //        LocationId = doctorProfile?.LocationId,
        //        CategoryId = doctorProfile?.CategoryId,
        //        AreaofExperties = doctorProfile?.AreaofExperties,
        //        Gender = doctorProfile?.Gender,
        //        About = doctorProfile?.About,
        //        EducationDetails = doctorProfile?.EducationDetails,
        //        ProfileImagePath = doctorProfile?.ProfileImagePath
        //    };

        //    return View(viewModel);
        //}

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

        #region Session Settings
        //public async Task<IActionResult> Settings()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (currentUser == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    var doctorId = currentUser.Id;
        //    var model = await _doctorProfileService.GetProfileSettingsAsync(doctorId);
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Settings(DoctorProfileSettingsVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    try
        //    {
        //        var success = await _doctorProfileService.SaveProfileSettingsAsync(model);

        //        if (success)
        //        {
        //            TempData["SuccessMessage"] = "Profile settings updated successfully!";
        //            return RedirectToAction(nameof(Settings));
        //        }

        //        ModelState.AddModelError("", "Failed to update profile settings.");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", $"An error occurred: {ex.Message}");
        //    }

        //    return View(model);
        //}

        #endregion

    }
}

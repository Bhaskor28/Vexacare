using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vexacare.Application.Categories;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.ServiceTypes;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IServiceTypeService _serviceTypeService;
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        #region Constructor
        public DoctorsController(
            ILocationService locationService,
            ICategoryService categoryService,
            IServiceTypeService serviceTypeService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
        {
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
            // Create view model with user data
            var viewModel = new ProfileBasicVM
            {
                UserId = currentUser.Id,
                Name = $"{currentUser.FirstName} {currentUser.LastName}",
                Email = currentUser.Email,
                //PhoneNumber = currentUser.PhoneNumber,
                // Map other properties as needed
            };

            return View(viewModel);
        }
        public async Task<IActionResult> EditProfileBasic()
        {
            // Get the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            ViewBag.Categories = await _categoryService.GetAllCategories();

            // Create view model with user data
            var viewModel = new ProfileBasicVM
            {
                UserId = currentUser.Id,
                Name = $"{currentUser.FirstName} {currentUser.LastName}",
                Email = currentUser.Email,
                //PhoneNumber = currentUser.PhoneNumber,
                // Map other properties as needed
            };

            return View(viewModel);
        }
        public async Task<IActionResult> EditProfileSession()
        {
            

            return View();
        }
    }
}

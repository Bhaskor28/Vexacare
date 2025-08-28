using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Categories;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.ServiceTypes;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly IServiceTypeService _serviceTypeService;
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        // Single constructor with all dependencies
        public ConsultationsController(
            IDoctorProfileService doctorProfileService,
            IServiceTypeService serviceTypeService,
            ILocationService locationService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             IMapper mapper)
        {
            _doctorProfileService = doctorProfileService;
            _serviceTypeService = serviceTypeService;
            _locationService = locationService;
            _categoryService = categoryService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int? categoryId, int? serviceTypeId, int? locationId, int? availableId)
        {
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            //var doctors = await _doctorProfileService.GetAllDoctorProfiles();

            // Get all categories, service types, and locations for dropdowns
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            ViewBag.Locations = await _locationService.GetAllLocationsAsync();
            // Get all doctors for total count
            var allDoctors = await _doctorProfileService.GetAllDoctorProfiles();
            ViewBag.TotalItem = allDoctors.Count();
            var doctorProfiles = await _doctorProfileService.GetFilteredDoctorProfilesAsync(categoryId,serviceTypeId,locationId,availableId);
            return View(doctorProfiles);
        }
        #region Profile
        public async Task<IActionResult> Profile(int id)
        {
            var doctor = await _doctorProfileService.GetDoctorProfileByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.ServiceTypes = await _serviceTypeService.GetAllServiceTypes();
            //ViewBag.Location = await _locationService.GetLocationByIdAsync(doctor.LocationId.Value);
            return View(doctor);
        }
        #endregion

        #region BookNow
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookNow(int id)
        {
            // Get the doctor by ID
            var doctor = await _doctorProfileService.GetDoctorProfileByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
        #endregion BookNow

        #region ConfirmPay
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> ConfirmPay(int id)
        {
            var doctor = await _doctorProfileService.GetDoctorProfileByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
        #endregion
        [Authorize(Roles = "Patient")]
        public IActionResult Confirmed()
        {
            return View();
        }

    }
}

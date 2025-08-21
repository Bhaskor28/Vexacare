using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly ILocationService _locationService;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Patient> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        // Single constructor with all dependencies
        public ConsultationsController(
            IDoctorProfileService doctorProfileService,
            ILocationService locationService,
            ILogger<HomeController> logger,
            UserManager<Patient> userManager,
            RoleManager<IdentityRole> roleManager,
             IMapper mapper)
        {
            _doctorProfileService = doctorProfileService;
            _locationService = locationService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }
            var doctors = await _doctorProfileService.GetAllDoctorProfiles();
            var locations = await _locationService.GetAllLocationsAsync();


            // Add doctors to ViewBag
            ViewBag.Doctors = doctors;
            ViewBag.Locations = locations;
            return View();
        }

        #region Profile
        public async Task<IActionResult> Profile(int id)
        {
            var doctor = await _doctorProfileService.GetDoctorProfileByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            
            var getDoctor = _mapper.Map<DoctorProfileVM>(doctor);

            return View(getDoctor);
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


            var getDoctor = _mapper.Map<DoctorProfileVM>(doctor);

            return View(getDoctor);
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


            var getDoctor = _mapper.Map<DoctorProfileVM>(doctor);

            return View(getDoctor);
        }
        #endregion
        [Authorize(Roles = "Patient")]
        public IActionResult Confirmed()
        {
            return View();
        }

    }
}

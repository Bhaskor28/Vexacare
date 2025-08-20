using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Patient> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        // Single constructor with all dependencies
        public ConsultationsController(
            ILogger<HomeController> logger,
            UserManager<Patient> userManager,
            RoleManager<IdentityRole> roleManager,
             IMapper mapper)
        {
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
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

            // Add doctors to ViewBag
            ViewBag.Doctors = doctors;
            return View();
        }

        #region Profile
        public async Task<IActionResult> Profile(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Check if the doctor is actually in the Doctor role
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");

            if (!isDoctor)
            {
                return NotFound();
            }

            // Pass doctor information to the view
            //ViewBag.Doctor = doctor;
            var doctorP = _mapper.Map<DoctorVM>(doctor);

            return View(doctorP);
        }
        #endregion

        #region BookNow
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookNow(string id)
        {
            // Get the doctor by ID
            var doctor = await _userManager.FindByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Check if the doctor is actually in the Doctor role
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");

            if (!isDoctor)
            {
                return NotFound();
            }

            // Pass doctor information to the view
            //ViewBag.Doctor = doctor;
            var doctorP = _mapper.Map<DoctorVM>(doctor);

            return View(doctorP);
        }
        #endregion BookNow

        #region ConfirmPay
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> ConfirmPay(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Check if the doctor is actually in the Doctor role
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");

            if (!isDoctor)
            {
                return NotFound();
            }

            // Pass doctor information to the view
            //ViewBag.Doctor = doctor;
            var doctorP = _mapper.Map<DoctorVM>(doctor);

            return View(doctorP);
        }
        #endregion
        [Authorize(Roles = "Patient")]
        public IActionResult Confirmed()
        {
            return View();
        }

    }
}

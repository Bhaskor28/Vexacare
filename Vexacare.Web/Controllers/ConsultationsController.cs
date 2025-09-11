using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using Vexacare.Application.Categories;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Application.ServiceTypes;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.DoctorEntities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        #region fields and constructor
        private readonly IMemoryCache _cache;
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
            IMemoryCache cache,
            IDoctorProfileService doctorProfileService,
            IServiceTypeService serviceTypeService,
            ILocationService locationService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             IMapper mapper)
        {
            _cache = cache;
            _doctorProfileService = doctorProfileService;
            _serviceTypeService = serviceTypeService;
            _locationService = locationService;
            _categoryService = categoryService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        #endregion

        #region Index
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
            var doctorProfiles = await _doctorProfileService.GetAllDoctorProfilesForPartnerHub();
            return View(doctorProfiles);
        }
        #endregion

        #region Profile
        public async Task<IActionResult> Profile(string id)
        {
            var existingDoctorProfile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(id);
            var existingDoctorSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(id);

            var partnerHubVM = new PartnerHubVM
            {
                ProfileBasic = existingDoctorProfile,
                ProfileSession = existingDoctorSession,
                

            };
            return View(partnerHubVM);
        }
        #endregion

        #region BookNow
        //[Authorize(Roles = "Patient")]

        [HttpGet]
        public async Task<IActionResult> BookNow(string id, DateTime? SelectedDate)
        {
            var existingDoctorProfile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(id);
            var existingDoctorSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(id);

            var partnerHubVM = new PartnerHubVM
            {
                ProfileBasic = existingDoctorProfile,
                ProfileSession = existingDoctorSession,
                SelectedDate = SelectedDate == null ? DateTime.Now : SelectedDate,
                SelectedDayName = SelectedDate?.ToString("dddd", CultureInfo.InvariantCulture),

            };
            return View(partnerHubVM);

        }
        [HttpPost]
        public async Task<IActionResult> BookNow(string id, DateTime? SelectedDate, List<TimeSpan>? SelectedTimeSlots)
        {
            // Get the doctor by ID
            var existingDoctorProfile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(id);
            var existingDoctorSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(id);

            var partnerHubVM = new PartnerHubVM
            {
                ProfileBasic = existingDoctorProfile,
                ProfileSession = existingDoctorSession,
                SelectedDate = SelectedDate==null?DateTime.Now:SelectedDate,
                SelectedDayName = SelectedDate?.ToString("dddd", CultureInfo.InvariantCulture),
                SelectedSlot = SelectedTimeSlots ?? new List<TimeSpan>()
            };
            // Generate a unique cache key
            var cacheKey = $"Booking_{id}_{User.Identity.Name}";
            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Cache expires after 30 minutes of inactivity
                .SetAbsoluteExpiration(TimeSpan.FromHours(2)); // Cache expires after 2 hours max

            // Store the VM in cache
            _cache.Set(cacheKey, partnerHubVM, cacheOptions);
            return View(partnerHubVM);
        }
        #endregion BookNow


        #region ConfirmPay
                
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ConfirmPay(string doctorId, DateTime? SelectedDate, List<TimeSpan> SelectedTimeSlots)
        {
            try
            {
                // Try to get from cache first
                var cacheKey = $"Booking_{doctorId}_{User.Identity.Name}";

                PartnerHubVM partnerHubVM;

                if (_cache.TryGetValue(cacheKey, out partnerHubVM) && partnerHubVM != null)
                {
                    // Update cached data with form values
                    if (SelectedDate.HasValue)
                        partnerHubVM.SelectedDate = SelectedDate.Value;

                    if (SelectedTimeSlots != null && SelectedTimeSlots.Any())
                        partnerHubVM.SelectedSlot = SelectedTimeSlots;
                }
                else
                {
                    // Fallback: get data from services
                    var existingDoctorProfile = await _doctorProfileService.GetDoctorProfileByUserIdAsync(doctorId);
                    var existingDoctorSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(doctorId);

                    partnerHubVM = new PartnerHubVM
                    {
                        ProfileBasic = existingDoctorProfile,
                        ProfileSession = existingDoctorSession,
                        SelectedDate = SelectedDate ?? DateTime.Now,
                        SelectedSlot = SelectedTimeSlots ?? new List<TimeSpan>(),
                        SelectedDayName = SelectedDate?.ToString("dddd", CultureInfo.InvariantCulture)
                    };

                    // Store in cache for future use
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                    _cache.Set(cacheKey, partnerHubVM, cacheOptions);
                }

                return View(partnerHubVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConfirmPay");
                TempData["Error"] = "An error occurred while processing your booking.";
                return RedirectToAction("Index");
            }
        }
        
        #endregion
        [Authorize(Roles = "Patient")]
        public IActionResult Confirmed()
        {
            return View();
        }


        //added by sazib or payment

        #region save checkout to cache
        // POST: Save Checkout Data to Cache
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> SaveCheckoutToCache(string doctorId, DateTime? SelectedDate, List<TimeSpan> SelectedTimeSlots)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                // Debug: log the received values
                _logger.LogInformation($"DoctorId: {doctorId}, Date: {SelectedDate}, Slots: {SelectedTimeSlots?.Count}");
                var cacheKey = $"Booking_{userId}";

                PartnerHubVM partnerHubVM = new PartnerHubVM();
                partnerHubVM.ProfileBasic = await _doctorProfileService.GetDoctorProfileByUserIdAsync(doctorId);
                partnerHubVM.ProfileSession = await _doctorProfileService.GetDoctorSessionByUserIdAsync(doctorId);

                if (partnerHubVM != null)
                {
                    // Update cached data with form values
                    if (SelectedDate.HasValue)
                        partnerHubVM.SelectedDate = SelectedDate.Value;

                    if (SelectedTimeSlots != null && SelectedTimeSlots.Any())
                        partnerHubVM.SelectedSlot = SelectedTimeSlots;

                    _cache.Set(cacheKey, partnerHubVM, TimeSpan.FromHours(1)); // Store for 1 hour
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Error saving booking data" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving booking data");
                return Json(new { success = false, message = "Error saving booking data" });
            }
        }
        #endregion

    }
}

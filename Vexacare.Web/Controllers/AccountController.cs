using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
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
                    return RedirectToAction("BasicInfo", "Account");
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
        public async Task<IActionResult> BasicInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var basicInfo = await _context.BasicInfos
                .FirstOrDefaultAsync(b => b.PatientId == patientId);

            if (basicInfo != null)
            {
                var model = new BasicInfoVM
                {
                    DateOfBirth = basicInfo.DateOfBirth,
                    Gender = basicInfo.Gender,
                    Country = basicInfo.Country,
                    City = basicInfo.City,
                    Postcode = basicInfo.Postcode
                    // ProfilePicture is not set here, as it's for uploads only
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BasicInfo(BasicInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);

                // Handle file upload
                string profilePictureUrl = null;
                if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(fileStream);
                    }

                    profilePictureUrl = "/uploads/" + uniqueFileName;
                }

                // Check if BasicInfo already exists for this patient
                var existingInfo = await _context.BasicInfos
                    .FirstOrDefaultAsync(b => b.PatientId == patientId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.ProfilePictureUrl = profilePictureUrl ?? existingInfo.ProfilePictureUrl;
                    existingInfo.DateOfBirth = model.DateOfBirth;
                    existingInfo.Gender = model.Gender;
                    existingInfo.Country = model.Country;
                    existingInfo.City = model.City;
                    existingInfo.Postcode = model.Postcode;

                    _context.BasicInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var basicInfo = new BasicInfo
                    {
                        PatientId = patientId,
                        ProfilePictureUrl = profilePictureUrl,
                        DateOfBirth = model.DateOfBirth,
                        Gender = model.Gender,
                        Country = model.Country,
                        City = model.City,
                        Postcode = model.Postcode
                    };

                    _context.BasicInfos.Add(basicInfo);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("HealthInfo", "Account"); // Redirect to next step
            }

            return View("BasicInfo", model);
        }

        //end of step 1
        //step 2: Health info

             
        [HttpGet]
        public async Task<IActionResult> HealthInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var healthInfo = await _context.HealthInfos
                .FirstOrDefaultAsync(b => b.PatientId == patientId);

            if (healthInfo != null)
            {
                var model = new HealthInfoVM
                {
                    Height = healthInfo.Height,
                    Weight = healthInfo.Weight,
                    BMI = healthInfo.BMI,
                    MainDiagnoses = healthInfo.MainDiagnoses,
                    DiagnosisDate = healthInfo.DiagnosisDate,
                    DrugName = healthInfo.DrugName,
                    Dosage = healthInfo.Dosage,
                    Frequency = healthInfo.Frequency,
                    StartDate = healthInfo.StartDate
                };
                
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HealthInfo(HealthInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                // Check if HealthInfo already exists for this patient
                var existingInfo = await _context.HealthInfos
                    .FirstOrDefaultAsync(h => h.PatientId == userId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.Height = model.Height;
                    existingInfo.Weight = model.Weight;
                    existingInfo.BMI = model.BMI; // Or calculate: (model.Weight / (model.Height * model.Height)) * 10000
                    existingInfo.MainDiagnoses = model.MainDiagnoses;
                    existingInfo.DiagnosisDate = model.DiagnosisDate;
                    existingInfo.DrugName = model.DrugName;
                    existingInfo.Dosage = model.Dosage;
                    existingInfo.Frequency = model.Frequency;
                    existingInfo.StartDate = model.StartDate;

                    _context.HealthInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var healthInfo = new HealthInfo
                    {
                        PatientId = userId,
                        Height = model.Height,
                        Weight = model.Weight,
                        BMI = model.BMI, // Or calculate here
                        MainDiagnoses = model.MainDiagnoses,
                        DiagnosisDate = model.DiagnosisDate,
                        DrugName = model.DrugName,
                        Dosage = model.Dosage,
                        Frequency = model.Frequency,
                        StartDate = model.StartDate
                    };

                    _context.HealthInfos.Add(healthInfo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("GastrointestinalInfo", "Account");
            }

            return View("HealthInfo", model);
        }
        //end of step 2

        //step 3: Gastrointestinal info

        [HttpGet]
        public IActionResult GastrointestinalInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GastrointestinalInfo(GastrointestinalInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var gastrointestinalInfo = new GastrointestinalInfo
                {
                    PatientId = userId,
                    PreviousGIProblems = model.PreviousGIProblems,
                    OnsetDateOfFirstSymptoms = model.OnsetDateOfFirstSymptoms,
                    TreatmentsPerformed = model.TreatmentsPerformed,
                    GIPathology = model.GIPathology,
                    DegreeOfRelationship = model.DegreeOfRelationship,
                    OtherRelevantMedicalConditions = model.OtherRelevantMedicalConditions,
                    TypeOfSurgery = model.TypeOfSurgery,
                    DateOfSurgery = model.DateOfSurgery,
                    Outcome = model.Outcome
                };

                _context.GastrointestinalInfos.Add(gastrointestinalInfo);
                await _context.SaveChangesAsync();

                return RedirectToAction("SymtomsInfo", "Account"); // Redirect to next step
            }

            return View(model);
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
        public async Task<IActionResult> LifestyleInfo()
        {
            //var patientId = _userManager.GetUserId(User);
            //var lifestyleInfo = await _context.LifestyleInfos
            //    .FirstOrDefaultAsync(l => l.PatientId == patientId);

            //if (lifestyleInfo != null)
            //{
            //    var model = new LifestyleInfoVM
            //    {
            //        //Id = lifestyleInfo.Id,
            //        ActivityType = lifestyleInfo.ActivityType,
            //        SessionsPerWeek = lifestyleInfo.SessionsPerWeek,
            //        AverageDurationMinutes = lifestyleInfo.AverageDurationMinutes,
            //        AverageHoursOfSleep = lifestyleInfo.AverageHoursOfSleep,
            //        SleepQualityRating = lifestyleInfo.SleepQualityRating,
            //        SleepProblems = lifestyleInfo.SleepProblems,
            //        StressLevel = lifestyleInfo.StressLevel,
            //        HasBreakfast = lifestyleInfo.HasBreakfast,
            //        CigarettesPerDay = lifestyleInfo.CigarettesPerDay
            //    };
            //    return View(model);
            //}

            //return View(new LifestyleInfoVM());
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> LifestyleInfo(LifestyleInfoVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var patientId = _userManager.GetUserId(User);

        //        if (model.Id > 0)
        //        {
        //            // Update existing record
        //            var existingInfo = await _context.LifestyleInfos.FindAsync(model.Id);
        //            if (existingInfo != null)
        //            {
        //                existingInfo.ActivityType = model.ActivityType;
        //                existingInfo.SessionsPerWeek = model.SessionsPerWeek;
        //                existingInfo.AverageDurationMinutes = model.AverageDurationMinutes;
        //                existingInfo.AverageHoursOfSleep = model.AverageHoursOfSleep;
        //                existingInfo.SleepQualityRating = model.SleepQualityRating;
        //                existingInfo.SleepProblems = model.SleepProblems;
        //                existingInfo.StressLevel = model.StressLevel;
        //                existingInfo.HasBreakfast = model.HasBreakfast;
        //                existingInfo.CigarettesPerDay = model.HasBreakfast == true ? model.CigarettesPerDay : null;

        //                _context.LifestyleInfos.Update(existingInfo);
        //            }
        //        }
        //        else
        //        {
        //            // Create new record
        //            var lifestyleInfo = new LifestyleInfo
        //            {
        //                PatientId = patientId,
        //                ActivityType = model.ActivityType,
        //                SessionsPerWeek = model.SessionsPerWeek,
        //                AverageDurationMinutes = model.AverageDurationMinutes,
        //                AverageHoursOfSleep = model.AverageHoursOfSleep,
        //                SleepQualityRating = model.SleepQualityRating,
        //                SleepProblems = model.SleepProblems,
        //                StressLevel = model.StressLevel,
        //                HasBreakfast = model.HasBreakfast,
        //                CigarettesPerDay = model.HasBreakfast == true ? model.CigarettesPerDay : null
        //            };

        //            _context.LifestyleInfos.Add(lifestyleInfo);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("TherapiesInfo", "Account"); // Proceed to next step
        //    }

        //    // If we got here, something went wrong
        //    return View(model);
        //}

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
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }
        //Sign Out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

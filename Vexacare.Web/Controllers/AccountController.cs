using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
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
                    await _userManager.AddToRoleAsync(user, "Patient");
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
        #endregion
        //end of step 1
        #region BasicInfo
        //step 1: basic info
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BasicInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var basicInfo = await _context.BasicInfos
                .FirstOrDefaultAsync(b => b.PatientId == patientId);

            var model = new BasicInfoVM();

            if (basicInfo != null)
            {
                model.DateOfBirth = basicInfo.DateOfBirth;
                model.Gender = basicInfo.Gender;
                model.Country = basicInfo.Country;
                model.City = basicInfo.City;
                model.Postcode = basicInfo.Postcode;
                model.ProfilePictureUrl = basicInfo.ProfilePictureUrl;
            }

            return View(model); // ✅ always returns non-null model
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
        #endregion
        //step 2: Health info
        #region HealthInfo

        [Authorize(Roles = "Patient")]

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
        #endregion

        //step 3: Gastrointestinal info
        #region GastrointestinalInfo
        
        public async Task<IActionResult> GastrointestinalInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var giInfo = await _context.GastrointestinalInfos
                .FirstOrDefaultAsync(g => g.PatientId == patientId);

            if (giInfo != null)
            {
                var model = new GastrointestinalInfoVM
                {
                    Id = giInfo.Id,
                    PreviousGIProblems = giInfo.PreviousGIProblems,
                    OnsetDateOfFirstSymptoms = giInfo.OnsetDateOfFirstSymptoms,
                    TreatmentsPerformed = giInfo.TreatmentsPerformed,
                    GIPathology = giInfo.GIPathology,
                    OtherRelevantMedicalConditions = giInfo.OtherRelevantMedicalConditions,
                    DegreeOfRelationship = giInfo.DegreeOfRelationship,
                    TypeOfSurgery = giInfo.TypeOfSurgery,
                    DateOfSurgery = giInfo.DateOfSurgery,
                    Outcome = giInfo.Outcome
                };
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GastrointestinalInfo(GastrointestinalInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                // Check if GastrointestinalInfo already exists for this patient
                var existingInfo = await _context.GastrointestinalInfos
                    .FirstOrDefaultAsync(g => g.PatientId == userId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.PreviousGIProblems = model.PreviousGIProblems;
                    existingInfo.OnsetDateOfFirstSymptoms = model.OnsetDateOfFirstSymptoms;
                    existingInfo.TreatmentsPerformed = model.TreatmentsPerformed;
                    existingInfo.GIPathology = model.GIPathology;
                    existingInfo.OtherRelevantMedicalConditions = model.OtherRelevantMedicalConditions;
                    existingInfo.DegreeOfRelationship = model.DegreeOfRelationship;
                    existingInfo.TypeOfSurgery = model.TypeOfSurgery;
                    existingInfo.DateOfSurgery = model.DateOfSurgery;
                    existingInfo.Outcome = model.Outcome;

                    _context.GastrointestinalInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var gastrointestinalInfo = new GastrointestinalInfo
                    {
                        PatientId = userId,
                        PreviousGIProblems = model.PreviousGIProblems,
                        OnsetDateOfFirstSymptoms = model.OnsetDateOfFirstSymptoms,
                        TreatmentsPerformed = model.TreatmentsPerformed,
                        GIPathology = model.GIPathology,
                        OtherRelevantMedicalConditions = model.OtherRelevantMedicalConditions,
                        DegreeOfRelationship = model.DegreeOfRelationship,
                        TypeOfSurgery = model.TypeOfSurgery,
                        DateOfSurgery = model.DateOfSurgery,
                        Outcome = model.Outcome
                    };

                    _context.GastrointestinalInfos.Add(gastrointestinalInfo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("SymptomsInfo", "Account");
            }

            // If model state is invalid, return to view with validation messages
            return View("GastrointestinalInfo", model);
        }

        #endregion
        //end of step 3

        //step 4: Symtoms info
        #region SymptomsInfo



        [HttpGet]
        public async Task<IActionResult> SymptomsInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var symptomsInfo = await _context.SymptomsInfos
                .FirstOrDefaultAsync(s => s.PatientId == patientId);
            if (symptomsInfo != null)
            {
                var model = new SymptomsInfoVM
                {
                    FrequencyOfEvaluations = symptomsInfo.FrequencyOfEvaluations,
                    BristolScale = symptomsInfo.BristolScale,
                    BloatingSeverity = symptomsInfo.BloatingSeverity,
                    IntestinalGas = symptomsInfo.IntestinalGas,
                    AbdominalPain = symptomsInfo.AbdominalPain,
                    DigestiveDifficulties = symptomsInfo.DigestiveDifficulties,
                    DiagnosedIntolerances = symptomsInfo.DiagnosedIntolerances,
                    CertifiedAllergies = symptomsInfo.CertifiedAllergies,
                    TestsPerformed = symptomsInfo.TestsPerformed
                };
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SymptomsInfo(SymptomsInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);
                // Check if SymptomsInfo already exists for this patient
                var existingInfo = await _context.SymptomsInfos
                    .FirstOrDefaultAsync(s => s.PatientId == patientId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.FrequencyOfEvaluations = model.FrequencyOfEvaluations;
                    existingInfo.BristolScale = model.BristolScale;
                    existingInfo.BloatingSeverity = model.BloatingSeverity;
                    existingInfo.IntestinalGas = model.IntestinalGas;
                    existingInfo.AbdominalPain = model.AbdominalPain;
                    existingInfo.DigestiveDifficulties = model.DigestiveDifficulties;
                    existingInfo.DiagnosedIntolerances = model.DiagnosedIntolerances;
                    existingInfo.CertifiedAllergies = model.CertifiedAllergies;
                    existingInfo.TestsPerformed = model.TestsPerformed;

                    _context.SymptomsInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var symptomsInfo = new SymptomsInfo
                    {
                        PatientId = patientId,
                        FrequencyOfEvaluations = model.FrequencyOfEvaluations,
                        BristolScale = model.BristolScale,
                        BloatingSeverity = model.BloatingSeverity,
                        IntestinalGas = model.IntestinalGas,
                        AbdominalPain = model.AbdominalPain,
                        DigestiveDifficulties = model.DigestiveDifficulties,
                        DiagnosedIntolerances = model.DiagnosedIntolerances,
                        CertifiedAllergies = model.CertifiedAllergies,
                        TestsPerformed = model.TestsPerformed
                    };
                    _context.SymptomsInfos.Add(symptomsInfo);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("DietProfileInfo", "Account");
            }
            return View("SymptomsInfo", model);
        }
        //end of step 4

        #endregion
        //end of step 4

        //step 5: DietProfile info
        #region DietProfileInfo
        
        public async Task<IActionResult> DietProfileInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var dietInfo = await _context.DietProfileInfos
                .FirstOrDefaultAsync(d => d.PatientId == patientId);

            if (dietInfo != null)
            {
                var model = new DietProfileInfoVM
                {
                    DietFood = dietInfo.DietFood,
                    DietTypeOther = dietInfo.DietTypeOther,
                    Vegetables = dietInfo.Vegetables,
                    Fruits = dietInfo.Fruits,
                    WholeGrains = dietInfo.WholeGrains,
                    AnimalProteins = dietInfo.AnimalProteins,
                    PlantProteins = dietInfo.PlantProteins,
                    DairyProducts = dietInfo.DairyProducts,
                    FermentedFoods = dietInfo.FermentedFoods,
                    Water = dietInfo.Water,
                    Alcohol = dietInfo.Alcohol,
                    BreakfastTime = dietInfo.BreakfastTime,
                    LunchTime = dietInfo.LunchTime,
                    DinnerTime = dietInfo.DinnerTime,
                    SnacksTime = dietInfo.SnacksTime

                };
                return View(model);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DietProfileInfo(DietProfileInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var patientId = _userManager.GetUserId(User);

                // Check if DietProfileInfo already exists for this patient
                var existingInfo = await _context.DietProfileInfos
                    .FirstOrDefaultAsync(d => d.PatientId == patientId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.DietFood = model.DietFood;
                    existingInfo.DietTypeOther = model.DietTypeOther;
                    existingInfo.Vegetables = model.Vegetables;
                    existingInfo.Fruits = model.Fruits;
                    existingInfo.WholeGrains = model.WholeGrains;
                    existingInfo.AnimalProteins = model.AnimalProteins;
                    existingInfo.PlantProteins = model.PlantProteins;
                    existingInfo.DairyProducts = model.DairyProducts;
                    existingInfo.FermentedFoods = model.FermentedFoods;
                    existingInfo.Water = model.Water;
                    existingInfo.Alcohol = model.Alcohol;
                    existingInfo.BreakfastTime = model.BreakfastTime;
                    existingInfo.LunchTime = model.LunchTime;
                    existingInfo.SnacksTime = model.SnacksTime;
                    existingInfo.DinnerTime = model.DinnerTime;

                    _context.DietProfileInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var dietInfo = new DietProfileInfo
                    {
                        PatientId = patientId,
                        DietFood = model.DietFood,
                        DietTypeOther = model.DietTypeOther,
                        Vegetables = model.Vegetables,
                        Fruits = model.Fruits,
                        WholeGrains = model.WholeGrains,
                        AnimalProteins = model.AnimalProteins,
                        PlantProteins = model.PlantProteins,
                        DairyProducts = model.DairyProducts,
                        FermentedFoods = model.FermentedFoods,
                        Water = model.Water,
                        Alcohol = model.Alcohol,
                        BreakfastTime = model.BreakfastTime,
                        LunchTime = model.LunchTime,
                        SnacksTime = model.SnacksTime,
                        DinnerTime = model.DinnerTime
                    };

                    _context.DietProfileInfos.Add(dietInfo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("LifestyleInfo", "Account"); // Update with your next step
            }

            return View("DietProfileInfo", model);
        }
        //end of step 5
        #endregion
        //end of step 4

        //step 6: Lifestyle info
        #region LifestyleInfo
        [HttpGet]
        public async Task<IActionResult> LifestyleInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var lifestyleInfo = await _context.LifestyleInfos
                .FirstOrDefaultAsync(l => l.PatientId == patientId);

            if (lifestyleInfo != null)
            {
                // Map existing info to view model
                var model = new LifestyleInfoVM
                {
                    Id = lifestyleInfo.Id,
                    ActivityType = lifestyleInfo.ActivityType,
                    SessionsPerWeek = lifestyleInfo.SessionsPerWeek,
                    AverageDurationMinutes = lifestyleInfo.AverageDurationMinutes,
                    AverageHoursOfSleep = lifestyleInfo.AverageHoursOfSleep,
                    SleepQualityRating = lifestyleInfo.SleepQualityRating,
                    SpecificProblems = lifestyleInfo.SpecificProblems,
                    StressLevel = lifestyleInfo.StressLevel,
                    IsSmoker = lifestyleInfo.IsSmoker,
                    CigarettesPerDay = lifestyleInfo.CigarettesPerDay
                };
                return View(model);
            }

            // Return view with new model with default values
            return View(new LifestyleInfoVM
            {
                SleepQualityRating = 5,
                StressLevel = 5
            });
        }

        [HttpPost]
        public async Task<IActionResult> LifestyleInfo(LifestyleInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                // Check if LifestyleInfo already exists for this patient
                var existingInfo = await _context.LifestyleInfos
                    .FirstOrDefaultAsync(l => l.PatientId == userId);

                if (existingInfo != null)
                {
                    // Update existing record
                    existingInfo.ActivityType = model.ActivityType;
                    existingInfo.SessionsPerWeek = model.SessionsPerWeek;
                    existingInfo.AverageDurationMinutes = model.AverageDurationMinutes;
                    existingInfo.AverageHoursOfSleep = model.AverageHoursOfSleep;
                    existingInfo.SleepQualityRating = model.SleepQualityRating;
                    existingInfo.SpecificProblems = model.SpecificProblems;
                    existingInfo.StressLevel = model.StressLevel;
                    existingInfo.IsSmoker = model.IsSmoker;
                    existingInfo.CigarettesPerDay = model.IsSmoker == true ? model.CigarettesPerDay : null;

                    _context.LifestyleInfos.Update(existingInfo);
                }
                else
                {
                    // Create new record
                    var lifestyleInfo = new LifestyleInfo
                    {
                        PatientId = userId,
                        ActivityType = model.ActivityType,
                        SessionsPerWeek = model.SessionsPerWeek,
                        AverageDurationMinutes = model.AverageDurationMinutes,
                        AverageHoursOfSleep = model.AverageHoursOfSleep,
                        SleepQualityRating = model.SleepQualityRating,
                        SpecificProblems = model.SpecificProblems,
                        StressLevel = model.StressLevel,
                        IsSmoker = model.IsSmoker,
                        CigarettesPerDay = model.IsSmoker == true ? model.CigarettesPerDay : null
                    };

                    _context.LifestyleInfos.Add(lifestyleInfo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("TherapiesInfo", "Account");
            }

            // If model state is invalid, return to view with validation messages
            return View("LifestyleInfo", model);
        }

        #endregion
        //end of step 6

        //step 7: Symtoms info

        // AccountController.cs
        [HttpGet]
        public async Task<IActionResult> TherapiesInfo()
        {
            var patientId = _userManager.GetUserId(User);
            var therapiesInfo = await _context.TherapiesInfos
                .FirstOrDefaultAsync(t => t.PatientId == patientId);

            var model = new TherapiesInfoVM();

            if (therapiesInfo != null)
            {
                model.UsedAntibioticsRecently = therapiesInfo.UsedAntibioticsRecently;
                model.AntibioticName = therapiesInfo.AntibioticName;
                model.EndOfTherapyDate = therapiesInfo.EndOfTherapyDate;
                model.UsesProbiotics = therapiesInfo.UsesProbiotics;
                model.UsesPrebiotics = therapiesInfo.UsesPrebiotics;
                model.UsesMinerals = therapiesInfo.UsesMinerals;
                model.UsesVitamins = therapiesInfo.UsesVitamins;
                model.UsesOtherSupplements = therapiesInfo.UsesOtherSupplements;
                model.OtherSupplementsDescription = therapiesInfo.OtherSupplementsDescription;

                if (therapiesInfo.PrimaryObjective.HasValue)
                {
                    model.PrimaryObjective = (PrimaryHealthObjective)therapiesInfo.PrimaryObjective.Value;
                }

                if (!string.IsNullOrEmpty(therapiesInfo.SecondaryObjectives))
                {
                    model.SecondaryObjectives = therapiesInfo.SecondaryObjectives
                        .Split(',')
                        .Select(o => (SecondaryObjective)Enum.Parse(typeof(SecondaryObjective), o))
                        .ToList();
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TherapiesInfo(TherapiesInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var therapiesInfo = await _context.TherapiesInfos
                    .FirstOrDefaultAsync(t => t.PatientId == userId) ?? new TherapiesInfo { PatientId = userId };

                therapiesInfo.UsedAntibioticsRecently = model.UsedAntibioticsRecently;
                therapiesInfo.AntibioticName = model.UsedAntibioticsRecently == true ? model.AntibioticName : null;
                therapiesInfo.EndOfTherapyDate = model.UsedAntibioticsRecently == true ? model.EndOfTherapyDate : null;
                therapiesInfo.UsesProbiotics = model.UsesProbiotics;
                therapiesInfo.UsesPrebiotics = model.UsesPrebiotics;
                therapiesInfo.UsesMinerals = model.UsesMinerals;
                therapiesInfo.UsesVitamins = model.UsesVitamins;
                therapiesInfo.UsesOtherSupplements = model.UsesOtherSupplements;
                therapiesInfo.OtherSupplementsDescription = model.UsesOtherSupplements ? model.OtherSupplementsDescription : null;
                therapiesInfo.PrimaryObjective = (int?)model.PrimaryObjective;
                therapiesInfo.SecondaryObjectives = model.SecondaryObjectives.Any() ?
                    string.Join(",", model.SecondaryObjectives.Select(o => o.ToString())) : null;

                if (therapiesInfo.Id == 0)
                {
                    _context.TherapiesInfos.Add(therapiesInfo);
                }
                else
                {
                    _context.TherapiesInfos.Update(therapiesInfo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "PatientDashboard");
            }

            return View(model);
        }
        //end of step 7

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
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}

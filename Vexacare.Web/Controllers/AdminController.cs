using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Text;
using System.Text.Json;
using Vexacare.Application.Interfaces;
using Vexacare.Application.MedicalReport;
using Vexacare.Application.Users.Doctors;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.MedicalReport;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.Stripe;
using Vexacare.Infrastructure.Services.StripeServices;

namespace Vexacare.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly StripeConfigService _stripeConfigService;

        private readonly IDoctorService _doctorService;

        private readonly IMedicalReportService _medicalReportService;
        private readonly ILogger<AdminController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        #region Constructor
        public AdminController(IDoctorService doctorService,
            StripeConfigService stripeConfigService,
            IMedicalReportService medicalReportService,
            ILogger<AdminController> logger,
            IConfiguration configuration,
            IOrderService orderService,
            UserManager<ApplicationUser> userManager
            )
        {
            _doctorService = doctorService;
            _stripeConfigService = stripeConfigService;
            _medicalReportService = medicalReportService;
            _logger = logger;
            _configuration = configuration;
            _orderService = orderService;
            _userManager = userManager;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        #region Doctor List
        [HttpGet]
        public async Task<IActionResult> DoctorList()
        {
            var doctors = await _doctorService.GetAllDoctorAsync();
            return View(doctors);
        }
        #endregion
        #region Register Doctor
        [HttpGet]
        public IActionResult RegisterDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorVM model)
        {
            //model.UserName = model.Email;
            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorService.AddDoctorAsync(model);
                    return RedirectToAction("DoctorList");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }
        #endregion
        #region Delete Doctor
        [HttpGet]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        [HttpPost, ActionName("DeleteDoctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctorConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var success = await _doctorService.DeleteDoctorAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Doctor deleted successfully.";
                return RedirectToAction("DoctorList");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete doctor.");
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            return View(doctor);
        }
        #endregion


        #region StripeAction
        public async Task<IActionResult> StripeSettings()
        {
            var currentConfig = await _stripeConfigService.GetConfigAsync() ?? new StripeConfig();
            return View(currentConfig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StripeSettings(StripeConfig model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _stripeConfigService.SaveConfigAsync(model);
                    ViewBag.SuccessMessage = "Stripe settings updated successfully!";
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving settings: {ex.Message}");
                }
            }
            return View(model);
        }
        #endregion

        #region OpenAISettings

        // Add these actions to your AdminController
        [HttpGet]
        public async Task<IActionResult> MedicalReportUpload()
        {
            // Check if API is available
            var isApiAvailable = await _medicalReportService.TestApiConnectionAsync();
            ViewBag.IsApiAvailable = isApiAvailable;
            ViewBag.ApiUrl = _configuration["ApiSettings:BaseUrl"];

            if (!isApiAvailable)
            {
                TempData["WarningMessage"] = "Medical Report API is not available. Please make sure the API project is running.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessMedicalReport(IFormFile medicalReport, int orderId)
        {
            // Check if API is available first
            var isApiAvailable = await _medicalReportService.TestApiConnectionAsync();
            if (!isApiAvailable)
            {
                TempData["ErrorMessage"] = "Medical Report API is not available. Please start the API project first.";
                return RedirectToAction("MedicalReportUpload");
            }

            if (medicalReport == null || medicalReport.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a medical report PDF file to upload.";
                return RedirectToAction("MedicalReportUpload");
            }

            if (!medicalReport.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Only PDF files are supported for medical reports.";
                return RedirectToAction("MedicalReportUpload");
            }

            try
            {
                _logger.LogInformation("Processing medical report: {FileName} for Order : {OrderId}", medicalReport.FileName, orderId);


                var order = await _orderService.GetOrderByIdAsync(orderId);

                
                var userAllInfo = await GetUserAllInfoByIdAsync(order.UserId);

                if (string.IsNullOrEmpty(userAllInfo))
                {
                    TempData["ErrorMessage"] = "User information not found.";
                    return RedirectToAction("MedicalReportUpload");
                }

                var result = await _medicalReportService.AnalyzeMedicalReportAsync(medicalReport, userAllInfo);

                if (result.Success)
                {
                    // Store the processed data in your database
                    await StoreMedicalReportAnalysis(result);

                    TempData["SuccessMessage"] = "Medical report processed successfully!";
                    TempData["ProcessedReport"] = JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    _logger.LogInformation("Medical report processed successfully: {FileName}", medicalReport.FileName);
                }
                else
                {
                    TempData["ErrorMessage"] = $"Error processing medical report: {result.ErrorMessage}";
                    _logger.LogWarning("Medical report processing failed: {Error}", result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                _logger.LogError(ex, "Error in ProcessMedicalReport action");
            }

            return RedirectToAction("MedicalReportUpload");
        }

        private async Task StoreMedicalReportAnalysis(MedicalReportAnalysisResult result)
        {
            // Implement your database storage logic here
            // Example:
            /*
            var medicalAnalysis = new MedicalReportAnalysisEntity
            {
                FileName = result.FileName,
                PatientAge = result.Analysis.PatientInformation.Age,
                PatientGender = result.Analysis.PatientInformation.Gender,
                KeyFindings = string.Join(";", result.Analysis.KeyFindings),
                Diagnoses = string.Join(";", result.Analysis.Diagnoses),
                Medications = string.Join(";", result.Analysis.Medications),
                RiskAssessment = result.Analysis.RiskAssessment,
                Summary = result.Analysis.Summary,
                ProcessedAt = result.ProcessedAt,
                RawAnalysis = JsonSerializer.Serialize(result.Analysis)
            };

            await _dbContext.MedicalReportAnalyses.AddAsync(medicalAnalysis);
            await _dbContext.SaveChangesAsync();
            */

            _logger.LogInformation("Medical report analysis stored for: {FileName}", result.FileName);
        }

        #endregion


        private async Task<string> GetUserAllInfoByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var userInfoBuilder = new StringBuilder();

                if (user != null)
                {
                    userInfoBuilder.AppendLine("=== USER CORE INFORMATION ===");
                    userInfoBuilder.AppendLine($"Name: {user.FirstName} {user.LastName}");
                    userInfoBuilder.AppendLine($"Email: {user.Email}");
                }

                //// Basic Information
                //if (basicInfo != null)
                //{
                //    userInfoBuilder.AppendLine("=== BASIC INFORMATION ===");
                //    userInfoBuilder.AppendLine($"Name: {basicInfo.FirstName} {basicInfo.LastName}");
                //    userInfoBuilder.AppendLine($"Age: {basicInfo.Age}");
                //    userInfoBuilder.AppendLine($"Gender: {basicInfo.Gender}");
                //    userInfoBuilder.AppendLine($"Date of Birth: {basicInfo.DateOfBirth:yyyy-MM-dd}");
                //    userInfoBuilder.AppendLine($"Contact: {basicInfo.PhoneNumber} | {basicInfo.Email}");
                //    userInfoBuilder.AppendLine($"Address: {basicInfo.Address}");
                //    userInfoBuilder.AppendLine();
                //}

                //// Medical Information
                //if (medicalInfo != null)
                //{
                //    userInfoBuilder.AppendLine("=== MEDICAL INFORMATION ===");
                //    userInfoBuilder.AppendLine($"Height: {medicalInfo.Height} cm");
                //    userInfoBuilder.AppendLine($"Weight: {medicalInfo.Weight} kg");
                //    userInfoBuilder.AppendLine($"BMI: {medicalInfo.BMI}");
                //    userInfoBuilder.AppendLine($"Blood Type: {medicalInfo.BloodType}");
                //    userInfoBuilder.AppendLine($"Chronic Conditions: {medicalInfo.ChronicConditions}");
                //    userInfoBuilder.AppendLine($"Previous Surgeries: {medicalInfo.PreviousSurgeries}");
                //    userInfoBuilder.AppendLine($"Current Health Status: {medicalInfo.CurrentHealthStatus}");
                //    userInfoBuilder.AppendLine();
                //}

                //// Lifestyle Information
                //if (lifestyleInfo != null)
                //{
                //    userInfoBuilder.AppendLine("=== LIFESTYLE INFORMATION ===");
                //    userInfoBuilder.AppendLine($"Activity Level: {lifestyleInfo.ActivityLevel}");
                //    userInfoBuilder.AppendLine($"Exercise Frequency: {lifestyleInfo.ExerciseFrequency}");
                //    userInfoBuilder.AppendLine($"Smoking Status: {lifestyleInfo.SmokingStatus}");
                //    userInfoBuilder.AppendLine($"Alcohol Consumption: {lifestyleInfo.AlcoholConsumption}");
                //    userInfoBuilder.AppendLine($"Sleep Pattern: {lifestyleInfo.SleepPattern}");
                //    userInfoBuilder.AppendLine($"Stress Level: {lifestyleInfo.StressLevel}");
                //    userInfoBuilder.AppendLine();
                //}

                //// Dietary Information
                //if (dietaryInfo != null)
                //{
                //    userInfoBuilder.AppendLine("=== DIETARY INFORMATION ===");
                //    userInfoBuilder.AppendLine($"Diet Type: {dietaryInfo.DietType}");
                //    userInfoBuilder.AppendLine($"Food Preferences: {dietaryInfo.FoodPreferences}");
                //    userInfoBuilder.AppendLine($"Restrictions: {dietaryInfo.DietaryRestrictions}");
                //    userInfoBuilder.AppendLine($"Average Calories: {dietaryInfo.AverageCaloricIntake}");
                //    userInfoBuilder.AppendLine($"Water Intake: {dietaryInfo.WaterIntake} liters/day");
                //    userInfoBuilder.AppendLine();
                //}

                //// Allergy Information
                //if (allergyInfo != null && allergyInfo.Any())
                //{
                //    userInfoBuilder.AppendLine("=== ALLERGIES ===");
                //    foreach (var allergy in allergyInfo)
                //    {
                //        userInfoBuilder.AppendLine($"- {allergy.AllergenName}: {allergy.Severity} (Reaction: {allergy.Reaction})");
                //    }
                //    userInfoBuilder.AppendLine();
                //}

                //// Current Medications
                //if (medicationInfo != null && medicationInfo.Any())
                //{
                //    userInfoBuilder.AppendLine("=== CURRENT MEDICATIONS ===");
                //    foreach (var medication in medicationInfo)
                //    {
                //        userInfoBuilder.AppendLine($"- {medication.MedicationName}: {medication.Dosage} {medication.Frequency}");
                //        if (!string.IsNullOrEmpty(medication.Purpose))
                //            userInfoBuilder.AppendLine($"  Purpose: {medication.Purpose}");
                //    }
                //    userInfoBuilder.AppendLine();
                //}

                //// Family History
                //if (familyHistory != null && familyHistory.Any())
                //{
                //    userInfoBuilder.AppendLine("=== FAMILY MEDICAL HISTORY ===");
                //    foreach (var history in familyHistory)
                //    {
                //        userInfoBuilder.AppendLine($"- {history.Relation}: {history.Condition} ({history.AgeAtDiagnosis} years)");
                //    }
                //    userInfoBuilder.AppendLine();
                //}

                return userInfoBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user basic information for user: {UserId}", userId);

                // Return minimal information if some services fail
                return $"User ID: {userId}. Error retrieving complete information: {ex.Message}";
            }
        }

    }
}

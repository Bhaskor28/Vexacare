
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using Vexacare.Application.Doctors.ViewModel;
using Vexacare.Application.MedicalReport;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Application.Users.Doctors;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities;
using Vexacare.Domain.Entities.MedicalReport;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.Stripe;
using Vexacare.Infrastructure.Data;
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

        #region Constructor
        public AdminController(IDoctorService doctorService,
            StripeConfigService stripeConfigService,
            IMedicalReportService medicalReportService,
            ILogger<AdminController> logger,
            IConfiguration configuration
            )
        {
            _doctorService = doctorService;
            _stripeConfigService = stripeConfigService;
            _medicalReportService = medicalReportService;
            _logger = logger;
            _configuration = configuration;
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
        public async Task<IActionResult> ProcessMedicalReport(IFormFile medicalReport)
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
                _logger.LogInformation("Processing medical report: {FileName}", medicalReport.FileName);

                var result = await _medicalReportService.AnalyzeMedicalReportAsync(medicalReport);

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

    }
}

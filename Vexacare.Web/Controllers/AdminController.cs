
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vexacare.Application.Doctors.ViewModel;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Application.Users.Doctors;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities;
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

        #region Constructor
        public AdminController(IDoctorService doctorService,
            StripeConfigService stripeConfigService)
        {
            _doctorService = doctorService;
            _stripeConfigService = stripeConfigService;
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
                    return View(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving settings: {ex.Message}");
                }
            }
            return View(model);
        }
        #endregion

    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        #region Constructor
        public AdminController(
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
        public IActionResult Index()
        {
            return View();
        }
        
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Application.Users.Doctors
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DoctorService(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<DoctorVM> AddDoctorAsync(DoctorVM doctor)
        {
            if (!await _roleManager.RoleExistsAsync("Doctor"))
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));

            var user = new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName
            };

            var result = await _userManager.CreateAsync(user, doctor.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
                await _context.SaveChangesAsync();

                return new DoctorVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }

            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IEnumerable<DoctorVM>> GetAllDoctorAsync()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            return doctors.Select(d => new DoctorVM
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber
            });
        }

        public async Task<DoctorVM> GetDoctorByIdAsync(string doctorId)
        {
            var doctor = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null) return null;

            return new DoctorVM
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber
            };
        }

        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null) return false;

            var roles = await _userManager.GetRolesAsync(doctor);
            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(doctor, roles);
            }

            var result = await _userManager.DeleteAsync(doctor);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

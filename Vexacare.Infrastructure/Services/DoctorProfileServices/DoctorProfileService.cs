using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.DoctorProfileServices
{
    public class DoctorProfileService : IDoctorProfileService
    {
        private readonly ApplicationDbContext _context;

        public DoctorProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorProfileVM>> GetAllDoctorProfiles()
        {
            var doctorProfiles = await _context.DoctorProfiles
                .Include(dp => dp.Category)
                .Include(dp => dp.ServiceType)
                .Include(dp => dp.Location)
                .Include(dp => dp.Availability)
                .Include(dp => dp.Reviews)
                .OrderBy(dp => dp.Name)
                .ToListAsync();

            return doctorProfiles.Select(doctor => new DoctorProfileVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                ProfilePictureUrl = doctor.ProfilePictureUrl,
                PatientsCount = doctor.PatientsCount,
                ConsultationType = doctor.ConsultationType,
                ConsultationFee = doctor.ConsultationFee,
                FeePeriod = doctor.FeePeriod,
                About = doctor.About,
                AreaofExperties = doctor.AreaofExperties,
                Languages = doctor.Languages,
                Reviews = doctor.Reviews,
                CategoryId = doctor.CategoryId,
                Category = doctor.Category,
                SubCategory = doctor.SubCategory,
                ServiceTypeId = doctor.ServiceTypeId,
                ServiceType = doctor.ServiceType,
                LocationId = doctor.LocationId,
                Location = doctor.Location,
                AvailabilityId = doctor.AvailabilityId,
                Availability = doctor.Availability
            });
        }
        public async Task<DoctorProfileVM> GetDoctorProfileByIdAsync(int doctorId)
        {
            var doctor = await _context.DoctorProfiles
                .Include(dp => dp.Category)
                .Include(dp => dp.ServiceType)
                .Include(dp => dp.Location)
                .Include(dp => dp.Availability)
                .Include(dp => dp.Reviews)
                .FirstOrDefaultAsync(dp => dp.Id == doctorId);

            if (doctor == null)
            {
                return null; // Or throw an exception if preferred
            }

            return new DoctorProfileVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                ProfilePictureUrl = doctor.ProfilePictureUrl,
                PatientsCount = doctor.PatientsCount,
                ConsultationType = doctor.ConsultationType,
                ConsultationFee = doctor.ConsultationFee,
                FeePeriod = doctor.FeePeriod,
                About = doctor.About,
                AreaofExperties = doctor.AreaofExperties,
                Languages = doctor.Languages,
                Reviews = doctor.Reviews,
                CategoryId = doctor.CategoryId,
                Category = doctor.Category,
                SubCategory = doctor.SubCategory,
                ServiceTypeId = doctor.ServiceTypeId,
                ServiceType = doctor.ServiceType,
                LocationId = doctor.LocationId,
                Location = doctor.Location,
                AvailabilityId = doctor.AvailabilityId,
                Availability = doctor.Availability
            };
        }

        public async Task<IEnumerable<DoctorProfileVM>> GetFilteredDoctorProfilesAsync(int? categoryId, int? serviceTypeId, int? locationId, int? availableId)
        {
            var query = _context.DoctorProfiles
        .Include(dp => dp.Category)
        .Include(dp => dp.ServiceType)
        .Include(dp => dp.Location)
        .Include(dp => dp.Availability)
        .Include(dp => dp.Reviews)
        .AsQueryable();
            
            // Apply filters only if they are provided
            if (categoryId.HasValue && categoryId>0)
            {
                query = query.Where(dp => dp.CategoryId == categoryId.Value);
            }
            if(serviceTypeId.HasValue && serviceTypeId > 0)
            {
                query = query.Where(dp => dp.ServiceTypeId == serviceTypeId.Value);

            }
            if (locationId.HasValue && locationId > 0)
            {
                query = query.Where(dp => dp.LocationId == locationId.Value);

            }
            if (availableId.HasValue && availableId > 0)
            {
                query = query.Where(dp => dp.AvailabilityId == availableId.Value);

            }
            var doctorProfiles = await query
        .OrderBy(dp => dp.Name)
        .ToListAsync();
            return doctorProfiles.Select(doctor => new DoctorProfileVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                ProfilePictureUrl = doctor.ProfilePictureUrl,
                PatientsCount = doctor.PatientsCount,
                ConsultationType = doctor.ConsultationType,
                ConsultationFee = doctor.ConsultationFee,
                FeePeriod = doctor.FeePeriod,
                About = doctor.About,
                AreaofExperties = doctor.AreaofExperties,
                Languages = doctor.Languages,
                Reviews = doctor.Reviews,
                CategoryId = doctor.CategoryId,
                Category = doctor.Category,
                SubCategory = doctor.SubCategory,
                ServiceTypeId = doctor.ServiceTypeId,
                ServiceType = doctor.ServiceType,
                LocationId = doctor.LocationId,
                Location = doctor.Location,
                AvailabilityId = doctor.AvailabilityId,
                Availability = doctor.Availability
            });
        }
    }
}

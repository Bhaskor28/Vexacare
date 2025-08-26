using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Domain.Entities.DoctorEntities;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.DoctorProfileServices
{
    public class DoctorProfileService : IDoctorProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private const string ContainerName = "doctors";

        public DoctorProfileService(ApplicationDbContext context,
            IFileStorageService fileStorageService, 
            IMapper mapper
            )
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task CreateDoctorBasicProfile(ProfileBasicVM model)
        {

            var existingDoctor = await _context.DoctorProfiles
                .FirstOrDefaultAsync(dp => dp.Id == model.Id);
            string imageUrl = null;
            if (model.DoctorImage != null)
            {
                if (existingDoctor != null && !string.IsNullOrEmpty(existingDoctor.ProfileImagePath))
                {
                    await _fileStorageService.DeleteFileAsync(existingDoctor.ProfileImagePath, ContainerName);
                }
                imageUrl = await _fileStorageService.SaveFileAsync(model.DoctorImage, ContainerName);
            }
            else if (existingDoctor != null)
            {
                // If no new image provided but updating, keep the existing image
                imageUrl = existingDoctor.ProfileImagePath;
            }
            if (existingDoctor != null)
            {
                // Update existing doctor profile
                _mapper.Map(model, existingDoctor); // Map from source to destination
                existingDoctor.ProfileImagePath = imageUrl;
                existingDoctor.ModifiedDate = DateTime.Now;

                _context.DoctorProfiles.Update(existingDoctor);
            }
            else
            {
                // Create new doctor profile
                var doctorBasicProfile = _mapper.Map<DoctorProfile>(model);
                doctorBasicProfile.ProfileImagePath = imageUrl;
                doctorBasicProfile.CreatedDate = DateTime.Now;
                doctorBasicProfile.ModifiedDate = DateTime.Now;

                await _context.DoctorProfiles.AddAsync(doctorBasicProfile);
            }
            
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<ProfileBasicVM>> GetAllDoctorProfiles()
        {
            var doctorProfiles = await _context.DoctorProfiles
                .Include(dp => dp.Category)
                .Include(dp => dp.ServiceType)
                .Include(dp => dp.Location)
                .Include(dp => dp.Reviews)
                .OrderBy(dp => dp.Name)
                .ToListAsync();

            var allDoctors = _mapper.Map<IEnumerable<ProfileBasicVM>>(doctorProfiles);
            return allDoctors;

            
        }

        public async Task<ProfileBasicVM> GetDoctorProfileByIdAsync(int doctorId)
        {
            var doctor = await _context.DoctorProfiles.FindAsync(doctorId);
            return _mapper.Map<ProfileBasicVM>(doctor);
        }

        public async Task<IEnumerable<ProfileBasicVM>> GetFilteredDoctorProfilesAsync(int? categoryId, int? serviceTypeId, int? locationId, int? availableId)
        {
            var query = _context.DoctorProfiles
                .Include(dp => dp.Category)
                .Include(dp => dp.ServiceType)
                .Include(dp => dp.Location)
                .Include(dp => dp.Reviews)
                .AsQueryable();

            // Apply filters
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(dp => dp.CategoryId == categoryId.Value);
            }

            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                query = query.Where(dp => dp.ServiceTypeId == serviceTypeId.Value);
            }

            if (locationId.HasValue && locationId.Value > 0)
            {
                query = query.Where(dp => dp.LocationId == locationId.Value);
            }

            
            var filteredProfiles = await query
                .OrderBy(dp => dp.Name)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProfileBasicVM>>(filteredProfiles);
        }
    }
}

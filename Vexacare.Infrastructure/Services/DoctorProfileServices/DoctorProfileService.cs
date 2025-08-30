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

            string imageUrl = null;
            if (model.DoctorImage != null)
            {
                imageUrl = await _fileStorageService.SaveFileAsync(model.DoctorImage, ContainerName);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await _fileStorageService.DeleteFileAsync(imageUrl, ContainerName);
                }
                imageUrl = await _fileStorageService.SaveFileAsync(model.DoctorImage, ContainerName);
            }

            var existingProfile = await _context.DoctorProfiles
         .FirstOrDefaultAsync(d => d.Id == model.Id);

            if (existingProfile != null)
            {
                // UPDATE: Map the values FROM the model TO the existing tracked entity
                _mapper.Map(model, existingProfile); // This is the key change!
                existingProfile.ProfileImagePath = imageUrl;
                existingProfile.ModifiedDate = DateTime.Now;
                // Don't update CreatedDate for existing records!

                // No need to call Update() since existingProfile is already tracked
                // _context.DoctorProfiles.Update(existingProfile); // REMOVE THIS LINE
            }
            else
            {
                // CREATE: Map to a new entity
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
            var doctor = await _context.DoctorProfiles
        .Include(dp => dp.Category)        // Include Category
        .Include(dp => dp.ServiceType)     // Include ServiceType
        .Include(dp => dp.Location)        // Include Location
        .Include(dp => dp.Reviews)         // Include Reviews
        .FirstOrDefaultAsync(dp => dp.Id == doctorId);

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

        //added by sazib
        // Add these new methods for availability settings
        public async Task<DoctorSessionVM> GetDoctorSessionByUserIdAsync(string doctorId)
        {
            var doctorProfile = await _context.DoctorProfiles
                .Include(d => d.Availabilities)
                .FirstOrDefaultAsync(d => d.UserId == doctorId);

            //if (doctorProfile == null)
            //    throw new ArgumentException("Doctor profile not found");

            var model = new DoctorSessionVM();
            if (doctorProfile != null)
            {
                model.DoctorProfileId = doctorProfile.Id;
                model.PricePerConsultation = doctorProfile.PricePerConsultation;
                model.SessionDuration = doctorProfile.SessionDuration;
                model.WeeklyAvailability = new List<DayAvailabilityVM>();


                // Get availability for each day of the week
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var availability = doctorProfile.Availabilities?
                        .FirstOrDefault(a => a.DayOfWeek == day);

                    model.WeeklyAvailability.Add(new DayAvailabilityVM
                    {
                        DayOfWeek = day,
                        StartTime = availability?.StartTime ?? new TimeSpan(9, 0, 0),
                        EndTime = availability?.EndTime ?? new TimeSpan(10, 0, 0),
                        IsAvailable = availability?.IsAvailable ?? false
                    });
                }
            }

            return model;
        }

        public async Task<bool> SaveProfileSettingsAsync(DoctorSessionVM model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Update doctor profile
                var doctorProfile = await _context.DoctorProfiles.FindAsync(model.DoctorProfileId);
                if (doctorProfile == null) return false;

                doctorProfile.PricePerConsultation = model.PricePerConsultation;
                doctorProfile.SessionDuration = model.SessionDuration;
                doctorProfile.ModifiedDate = DateTime.UtcNow;

                // Update availability
                var existingAvailabilities = await _context.Availabilities
                    .Where(a => a.DoctorProfileId == model.DoctorProfileId)
                    .ToListAsync();

                _context.Availabilities.RemoveRange(existingAvailabilities);

                foreach (var dayAvailability in model.WeeklyAvailability)
                {
                    if (dayAvailability.IsAvailable)
                    {
                        _context.Availabilities.Add(new Availability
                        {
                            DoctorProfileId = model.DoctorProfileId,
                            DayOfWeek = dayAvailability.DayOfWeek,
                            StartTime = dayAvailability.StartTime,
                            EndTime = dayAvailability.EndTime,
                            IsAvailable = true,
                            SlotDuration = model.SessionDuration
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task InitializeDefaultAvailabilityAsync(int doctorProfileId)
        {
            // Create default availability (Monday-Friday, 9 AM - 5 PM)
            var availabilities = new List<Availability>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                // Set default availability for weekdays
                bool isAvailable = day >= DayOfWeek.Monday && day <= DayOfWeek.Friday;

                availabilities.Add(new Availability
                {
                    DoctorProfileId = doctorProfileId,
                    DayOfWeek = day,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IsAvailable = isAvailable,
                    SlotDuration = 60
                });
            }

            await _context.Availabilities.AddRangeAsync(availabilities);
            await _context.SaveChangesAsync();
        }

        public async Task<ProfileBasicVM> GetDoctorProfileByUserIdAsync(string doctorId)
        {
            var doctor = await _context.DoctorProfiles
            .Include(dp => dp.Category)        // Include Category
            .Include(dp => dp.ServiceType)     // Include ServiceType
            .Include(dp => dp.Location)        // Include Location
            .Include(dp => dp.Reviews)         // Include Reviews
            .FirstOrDefaultAsync(dp => dp.UserId == doctorId);

            return _mapper.Map<ProfileBasicVM>(doctor);
        }


    }
}

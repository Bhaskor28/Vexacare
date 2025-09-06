using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Patients.ViewModels;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.PatientProfileServices
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        private const string ContainerName = "profile";

        public PatientService(
            ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IMapper mapper
            )
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<BasicInfoVM> GetBasicInfoAsync(string patientId)
        {
            var basicInfo = await _context.BasicInfos.FirstOrDefaultAsync(b => b.PatientId == patientId);
            var model = _mapper.Map<BasicInfoVM>(basicInfo);
            return model;
        }

        public async Task<bool> SaveBasicInfoAsync(string patientId, BasicInfoVM model)
        {
            try
            {
                string imageUrl = model.ProfilePictureUrl;
                if (model.ProfilePicture != null)
                {
                    imageUrl = await _fileStorageService.SaveFileAsync(model.ProfilePicture, ContainerName);
                }
                model.ProfilePictureUrl = imageUrl;

                var existingInfo = await _context.BasicInfos
                    .FirstOrDefaultAsync(b => b.PatientId == patientId);

                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.BasicInfos.Update(existingInfo);
                }
                else
                {
                    var basicInfo = _mapper.Map<BasicInfo>(model);
                    basicInfo.PatientId = patientId;
                    _context.BasicInfos.Add(basicInfo);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<HealthInfoVM> GetHealthInfoAsync(string patientId)
        {
            var healthInfo = await _context.HealthInfos.FirstOrDefaultAsync(h => h.PatientId == patientId);
            var model = _mapper.Map<HealthInfoVM>(healthInfo);
            return model;
        }
        public async Task<bool> SaveHealthInfoAsync(string patientId, HealthInfoVM model)
        {
            try
            {
                var existingInfo = await _context.HealthInfos
                    .FirstOrDefaultAsync(h => h.PatientId == patientId);

                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.HealthInfos.Update(existingInfo);
                }
                else
                {
                    var healthInfo = _mapper.Map<HealthInfo>(model);
                    healthInfo.PatientId = patientId;
                    _context.HealthInfos.Add(healthInfo);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<GastrointestinalInfoVM> GetGastrointestinalInfoAsync(string patientId)
        {
            var gastrointestinalInfo = await _context.GastrointestinalInfos
                .FirstOrDefaultAsync(g => g.PatientId == patientId);
            var model = _mapper.Map<GastrointestinalInfoVM>(gastrointestinalInfo);
            return model;
        }
        public async Task<bool> SaveGastrointestinalInfoAsync(string patientId, GastrointestinalInfoVM model)
        {
            try
            {
                var existingInfo = await _context.GastrointestinalInfos
                    .FirstOrDefaultAsync(g => g.PatientId == patientId);
                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.GastrointestinalInfos.Update(existingInfo);
                }
                else
                {
                    var gastrointestinalInfo = _mapper.Map<GastrointestinalInfo>(model);
                    gastrointestinalInfo.PatientId = patientId;
                    _context.GastrointestinalInfos.Add(gastrointestinalInfo);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<SymptomsInfoVM> GetSymptomsInfoAsync(string patientId)
        {
            var symptomsInfo =  await _context.SymptomsInfos
                .FirstOrDefaultAsync(s => s.PatientId == patientId);
            var model = _mapper.Map<SymptomsInfoVM>(symptomsInfo);
            return model;
        }
        public async Task<bool> SaveSymptomsInfoAsync(string patientId, SymptomsInfoVM model)
        {
            try
            {
                var existingInfo = await _context.SymptomsInfos
                    .FirstOrDefaultAsync(s => s.PatientId == patientId);
                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.SymptomsInfos.Update(existingInfo);
                }
                else
                {
                    var symptomsInfo = _mapper.Map<SymptomsInfo>(model);
                    symptomsInfo.PatientId = patientId;
                    _context.SymptomsInfos.Add(symptomsInfo);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DietProfileInfoVM> GetDietProfileInfoAsync(string patientId)
        {
            var dietInfo =  await _context.DietProfileInfos
                .FirstOrDefaultAsync(d => d.PatientId == patientId);
            var model = _mapper.Map<DietProfileInfoVM>(dietInfo);
            return model;
        }
        public async Task<bool> SaveDietProfileInfoAsync(string patientId, DietProfileInfoVM model)
        {
            try
            {
                var existingInfo = await _context.DietProfileInfos
                    .FirstOrDefaultAsync(d => d.PatientId == patientId);

                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.DietProfileInfos.Update(existingInfo);
                }
                else
                {
                    var dietInfo = _mapper.Map<DietProfileInfo>(model);
                    dietInfo.PatientId = patientId;
                    _context.DietProfileInfos.Add(dietInfo);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<LifestyleInfoVM> GetLifestyleInfoAsync(string patientId)
        {
            var lifestyleInfo =  await _context.LifestyleInfos
                .FirstOrDefaultAsync(l => l.PatientId == patientId);
            var model = _mapper.Map<LifestyleInfoVM>(lifestyleInfo);
            return model;
        }
        public async Task<bool> SaveLifestyleInfoAsync(string patientId, LifestyleInfoVM model)
        {
            try
            {
                var existingInfo = await _context.LifestyleInfos
                    .FirstOrDefaultAsync(l => l.PatientId == patientId);

                if (existingInfo != null)
                {
                    _mapper.Map(model, existingInfo);
                    _context.LifestyleInfos.Update(existingInfo);
                }
                else
                {
                    var lifestyleInfo = _mapper.Map<LifestyleInfo>(model);
                    lifestyleInfo.PatientId = patientId;
                    _context.LifestyleInfos.Add(lifestyleInfo);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TherapiesInfoVM> GetTherapiesInfoAsync(string patientId)
        {
            return null;
        }
        public async Task<bool> SaveTherapiesInfoAsync(string patientId, TherapiesInfoVM model)
        {
            return false;
        }
    }
}

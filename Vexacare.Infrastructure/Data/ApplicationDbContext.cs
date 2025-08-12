using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Patient>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BasicInfo> BasicInfos { get; set; }
        public DbSet<HealthInfo> HealthInfos { get; set; }
        public DbSet<GastrointestinalInfo> GastrointestinalInfos { get; set; }
        public DbSet<DietProfileInfo> DietProfileInfos { get; set; }
        public DbSet<SymptomsInfo> SymptomsInfos { get; set; }
        public DbSet<LifestyleInfo> LifestyleInfos { get; set; }
        public DbSet<TherapiesInfo> TherapiesInfos { get; set; }

    }
}

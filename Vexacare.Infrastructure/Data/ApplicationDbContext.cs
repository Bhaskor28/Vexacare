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
    }
}

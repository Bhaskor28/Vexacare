using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data.Configurations.Product;

namespace Vexacare.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Patient>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Patient Tables
        public DbSet<BasicInfo> BasicInfos { get; set; }
        public DbSet<HealthInfo> HealthInfos { get; set; }
        public DbSet<GastrointestinalInfo> GastrointestinalInfos { get; set; }
        public DbSet<DietProfileInfo> DietProfileInfos { get; set; }
        public DbSet<SymptomsInfo> SymptomsInfos { get; set; }
        public DbSet<LifestyleInfo> LifestyleInfos { get; set; }
        public DbSet<TherapiesInfo> TherapiesInfos { get; set; }
        

        //product Tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<ProductBenefit> ProductBenefits { get; set; } //mapping table of products and benefist.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductBenefitConfiguration());
        }

    }
}

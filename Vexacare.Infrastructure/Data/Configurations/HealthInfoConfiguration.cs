using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data.Configurations
{
    public class HealthInfoConfiguration
    {
        public void Configure(EntityTypeBuilder<HealthInfo> builder)
        {
            // One-to-one relationship with Patient
            builder.HasOne(h => h.Patient)
                .WithOne()
                .HasForeignKey<HealthInfo>(h => h.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

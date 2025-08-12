using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data.Configurations
{
    public class SymptomsInfoConfiguration
    {
        public void Configure(EntityTypeBuilder<SymptomsInfo> builder)
        {
            // One-to-one relationship with Patient
            builder.HasOne(s => s.Patient)
                .WithOne()
                .HasForeignKey<SymptomsInfo>(s => s.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

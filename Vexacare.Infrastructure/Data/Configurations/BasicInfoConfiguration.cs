using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data.Configurations
{
    public class BasicInfoConfiguration
    {
        public void Configure(EntityTypeBuilder<BasicInfo> builder)
        {
            builder.HasOne(b => b.Patient)
                .WithOne() // <-- This sets up a one-to-one relationship
                .HasForeignKey<BasicInfo>(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
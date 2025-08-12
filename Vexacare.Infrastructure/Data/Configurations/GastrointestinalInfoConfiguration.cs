using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data.Configurations
{
    public class GastrointestinalInfoConfiguration
    {
        public void Configure(EntityTypeBuilder<GastrointestinalInfo> builder)
        {
            // One-to-one relationship with Patient
            builder.HasOne(h => h.Patient)
                .WithOne()
                .HasForeignKey<HealthInfo>(h => h.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

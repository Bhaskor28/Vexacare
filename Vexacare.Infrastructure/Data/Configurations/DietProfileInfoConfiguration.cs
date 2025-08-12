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
    public class DietProfileInfoConfiguration
    {
        public void Configure(EntityTypeBuilder<DietProfileInfo> builder)
        {
            // One-to-one relationship with Patient
            builder.HasOne(d => d.Patient)
                .WithOne()
                .HasForeignKey<DietProfileInfo>(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
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
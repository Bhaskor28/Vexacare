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
    public class BasicInfoConfiguration : IEntityTypeConfiguration<BasicInfo>
    {
        public void Configure(EntityTypeBuilder<BasicInfo> builder)
        {
            //builder.ToTable("BasicInfo");

            //builder.HasKey(b => b.Id);

            //builder.Property(b => b.ProfilePictureUrl)
            //    .HasMaxLength(255);

            //builder.Property(b => b.Gender)
            //    .HasMaxLength(50);

            //builder.Property(b => b.Country)
            //    .HasMaxLength(100);

            //builder.Property(b => b.City)
            //    .HasMaxLength(100);

            //builder.Property(b => b.Postcode)
            //    .HasMaxLength(20);

            // Relationship with Patient
            builder.HasOne(b => b.Patient)
                .WithOne() // <-- This sets up a one-to-one relationship
                .HasForeignKey<BasicInfo>(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
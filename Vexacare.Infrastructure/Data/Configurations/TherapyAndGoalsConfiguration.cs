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
    public class TherapyAndGoalsConfiguration : IEntityTypeConfiguration<TherapiesInfo>
    {
        
        public void Configure(EntityTypeBuilder<TherapiesInfo> builder)
        {
            

        }
    }
}

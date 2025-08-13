using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Infrastructure.Data.Configurations.Product
{
    public class ProductBenefitConfiguration : IEntityTypeConfiguration<ProductBenefit>
    {
        public void Configure(EntityTypeBuilder<ProductBenefit> builder)
        {
            // composite primary key
            builder.HasKey(pb => new { pb.ProductId, pb.BenefitId });

            // relationships
            builder.HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBenefits)
                .HasForeignKey(pb => pb.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pb => pb.Benefit)
                .WithMany(b => b.ProductBenefits)
                .HasForeignKey(pb => pb.BenefitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

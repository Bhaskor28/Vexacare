using AutoMapper;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Locations;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.UsersVM;
using Vexacare.Domain.Entities.DoctorEntities;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region by Sazib
            // Product mappings
            CreateMap<Product, ProductListVM>()
                .ForMember(dest => dest.ProductBenefits,
                           opt => opt.MapFrom(src => src.ProductBenefits));

            // ProductBenefit -> BenefitVM mapping
            CreateMap<ProductBenefit, BenefitVM>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.Benefit.Id))
                .ForMember(dest => dest.BenefitName,
                           opt => opt.MapFrom(src => src.Benefit.BenefitName));

            
            CreateMap<Product, EditProductVM>()
                .ForMember(dest => dest.SelectedBenefitIds,
                           opt => opt.MapFrom(src => src.ProductBenefits.Select(pb => pb.BenefitId).ToList())
                )
                .ForMember(dest => dest.ProductImagePath,
                           opt => opt.MapFrom(src => src.ProductImages)
                )
                .AfterMap((src, dest) =>
                {
                    foreach (var benefit in dest.AvailableBenefits)
                    {
                        benefit.IsSelected = dest.SelectedBenefitIds.Contains(benefit.Id);
                    }
                }
            );
           
            // Reverse mapping for update operations
            CreateMap<EditProductVM, Product>()
                .ForMember(dest => dest.ProductBenefits,
                    opt => opt.Ignore()) // We'll handle this separately
                .ForMember(dest => dest.ProductImages,
                    opt => opt.Condition(src => src.ProductImage == null)); // Keep existing if no new image


            CreateMap<Product, ProductDetailsVM>()
                .ForMember(dest => dest.ProductImagePath, otp => otp.MapFrom(src=>src.ProductImages))
                .ForMember(dest => dest.ProductBenefits,
                           opt => opt.MapFrom(src => src.ProductBenefits));

            CreateMap<AddProductVM, Product>()
                .ForMember(dest => dest.ProductBenefits,
                           opt => opt.Ignore());

            

            // Benefit mappings
            CreateMap<Benefit, BenefitVM>();

            CreateMap<BenefitVM, Benefit>()
            .ForMember(dest => dest.ProductBenefits, opt => opt.Ignore());
            #endregion


            #region by Bhaskor
            CreateMap<DoctorVM, Patient>().ReverseMap();
            CreateMap<LocationVM, Location>().ReverseMap();
            CreateMap<DoctorProfileVM, DoctorProfile>().ReverseMap();
            #endregion

        }
        



    }
}
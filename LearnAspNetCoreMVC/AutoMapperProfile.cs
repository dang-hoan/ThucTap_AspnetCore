using AutoMapper;
using LearnAspNetCoreMVC.Models;

namespace LearnAspNetCoreMVC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //<Source, Destination> : Convert Source -> Destination
            CreateMap<CompanyPatch, Company>()
                .ForMember(
                    dest => dest.CreateDate,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.UpdateDate,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.DeleteDate,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.IsDelete,
                    opt => opt.Ignore()
                );
        }
    }
}

using AutoMapper;
using LearnAspNetCoreMVC.Models;

namespace LearnAspNetCoreMVC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //<Source, Destination> : Convert Source -> Destination
            CreateMap<IEnumerable<Category>, CategoryViewModel>()
                .ForMember(
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => src)
                );
        }
    }
}

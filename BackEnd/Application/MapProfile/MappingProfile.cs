using Application.DTO;
using AutoMapper;
using Domain;

namespace Application.MapProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.Id));
        }
        
    }
}

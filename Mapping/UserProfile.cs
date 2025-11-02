using AutoMapper;
using gozba_na_klik.Dtos;
using gozba_na_klik.Model;

namespace gozba_na_klik.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.Allergens,
                           opt => opt.MapFrom(src => src.UserAllergens
                               .Select(ua => ua.Allergen.Name).ToList()))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));
        }
    }
}

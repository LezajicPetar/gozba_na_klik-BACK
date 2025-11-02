using AutoMapper;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Dtos;
using gozba_na_klik.Model;

namespace gozba_na_klik.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, AdminUserDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));

            CreateMap<CreateUserDto, User>();

            CreateMap<User, OwnerDto>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.Allergens,
                           opt => opt.MapFrom(src => src.UserAllergens
                               .Select(ua => ua.Allergen.Name).ToList()))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));
        }
    }
}

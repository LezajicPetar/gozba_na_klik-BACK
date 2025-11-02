using AutoMapper;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
using System.Net.Sockets;

namespace gozba_na_klik.Mapping
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile() 
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(
                dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner.FirstName + " " + src.Owner.LastName))
                .ForMember(
                dest => dest.Menu,
                opt => opt.MapFrom(src => src.Menu));
        }
    }
}

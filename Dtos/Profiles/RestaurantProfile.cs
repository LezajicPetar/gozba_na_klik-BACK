using AutoMapper;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;

namespace gozba_na_klik.Dtos.Profiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            // Summary
            CreateMap<Restaurant, RestaurantSummaryDto>()
                .ForMember(d => d.Photo, o => o.MapFrom(s => s.Photo));

            // Details
            CreateMap<Restaurant, RestaurantDetailsDto>()
                .ForMember(d => d.OwnerName, o =>
                    o.MapFrom(s =>
                    s.Owner != null
                        ? s.Owner.FirstName + " " + s.Owner.LastName
                        : string.Empty));



            // Create/Update
            CreateMap<RestaurantUpsertDto, Restaurant>()
                .ForAllMembers(o => o.Condition((src, dest, val) => val != null));

            // Work time
            CreateMap<RestaurantWorkTime, RestaurantWorkTimeDto>()
                .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.Open.ToString(@"hh\:mm\:ss")))
                 .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.Close.ToString(@"hh\:mm\:ss")))
                 .ReverseMap()
                 .ForMember(dest => dest.Open, opt => opt.MapFrom(src =>
                     string.IsNullOrEmpty(src.Open) ? TimeSpan.Zero : TimeSpan.Parse(src.Open)))
                 .ForMember(dest => dest.Close, opt => opt.MapFrom(src =>
                     string.IsNullOrEmpty(src.Close) ? TimeSpan.Zero : TimeSpan.Parse(src.Close)));


            // Exception dates
            CreateMap<RestaurantExceptionDate, RestaurantExceptionDto>().ReverseMap();
        }
    }
}

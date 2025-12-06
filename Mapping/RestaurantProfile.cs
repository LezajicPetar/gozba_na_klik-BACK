using AutoMapper;
using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Mapping
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            // LIST / SUMMARY
            CreateMap<Restaurant, RestaurantSummaryDto>()
                .ForMember(d => d.Photo, o => o.MapFrom(s => s.Photo))
                .ForMember(d => d.isPublished, o => o.MapFrom(s => s.Menu != null && s.Menu.Any()))
                .ForMember(d => d.Menu, o => o.MapFrom(s => s.Menu));

            // DETAILS
            CreateMap<Restaurant, RestaurantDetailsDto>()
                .ForMember(d => d.OwnerName, o =>
                    o.MapFrom(s => s.Owner != null
                        ? s.Owner.FirstName + " " + s.Owner.LastName
                        : string.Empty));

            // FULL LIST DTO
            CreateMap<Restaurant, RestaurantDto>()
               .ForMember(d => d.OwnerName,
                   o => o.MapFrom(s => s.Owner == null
                       ? string.Empty
                       : $"{s.Owner.FirstName} {s.Owner.LastName}".Trim()))
               .ForMember(d => d.Rating,
               opt => opt.MapFrom(src => src.Reviews.Count == 0
                    ? 0
                    : src.Reviews.Average(r => r.Rating)));

            // UPSERT / INPUT
            CreateMap<RestaurantUpsertDto, Restaurant>()
                .ForAllMembers(o => o.Condition((src, dest, val) => val != null));

            CreateMap<RestaurantInputDto, Restaurant>();

            // WORKTIME
            CreateMap<RestaurantWorkTime, RestaurantWorkTimeDto>()
                .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.Open.ToString(@"hh\:mm\:ss")))
                .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.Close.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(dest => dest.Open, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Open) ? TimeSpan.Zero : TimeSpan.Parse(src.Open)))
                .ForMember(dest => dest.Close, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Close) ? TimeSpan.Zero : TimeSpan.Parse(src.Close)));

            // EXCEPTIONS
            CreateMap<RestaurantExceptionDate, RestaurantExceptionDto>().ReverseMap();
        }
    }
}

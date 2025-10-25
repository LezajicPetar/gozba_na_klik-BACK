﻿using AutoMapper;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;

namespace gozba_na_klik.Mapping
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile() 
        {
            CreateMap<Restaurant, RestaurantDto>()
               .ForMember(d => d.OwnerName,
                   o => o.MapFrom(s => s.Owner == null
                       ? string.Empty
                       : $"{s.Owner.FirstName} {s.Owner.LastName}".Trim()));

            CreateMap<RestaurantInputDto, Restaurant>();
        }
    }
}

using AutoMapper;
using gozba_na_klik.Dtos.MenuItem;
using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Model;

namespace gozba_na_klik.Mapping
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDto>().ReverseMap();
            CreateMap<MenuItem, UpdateMenuItemDto>().ReverseMap();
            CreateMap<MenuItem, CreateMenuItemDto>().ReverseMap();
            CreateMap<MenuItem, ReadMenuItemDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<CreateOrderDto, Order>().ReverseMap();
        }
    }
}

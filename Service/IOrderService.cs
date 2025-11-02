using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Dtos.Users;

namespace gozba_na_klik.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
    }
}

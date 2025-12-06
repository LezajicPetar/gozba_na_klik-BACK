using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Dtos.Users;

namespace gozba_na_klik.Service.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task AcceptAsync(int orderId); // Restoran prihvata porudzbinu AZ
        Task RejectAsync(int orderId, RejectOrderDto? dto = null); // Restoran odbija porudzbinu AZ
        Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<OrderDto>> GetPendingAsync(CancellationToken ct = default);
        Task<IEnumerable<OrderDto>> GetByCustomerAsync(int customerId);
        Task<List<OrderDto>> GetPendingForOwnerAsync(int ownerId, CancellationToken ct = default); // Dodao sam za listu porudzbina vlasnika AZ
        Task AssignCourierAsync(int orderId, int courierId);

    }
}

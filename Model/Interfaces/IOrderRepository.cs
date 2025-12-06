using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Model.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<Order> UpdateAsync(Order order); // Dodao sam UpdateAsync metodu AZ
        Task<List<Order>> GetPendingAsync(CancellationToken ct = default);
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
        Task<List<Order>> GetPendingForOwnerAsync(int ownerId, CancellationToken ct = default); // Dodao sam za listu porudzbina AZ
        Task AssignCourierAsync(int orderId, int courierId);

    }
}

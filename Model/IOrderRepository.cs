namespace gozba_na_klik.Model
{
    public interface IOrderRepository
    {
         Task<Order> CreateAsync(Order order);
         Task<Order?> GetByIdAsync(int id);

    }
}

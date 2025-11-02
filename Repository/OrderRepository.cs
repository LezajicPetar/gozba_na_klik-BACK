using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GozbaDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(GozbaDbContext dbContext, ILogger<OrderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

    }
}

using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Enums;
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
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .Include(o => o.Address)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order> UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
        public async Task<List<Order>> GetPendingAsync(CancellationToken ct = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == OrderStatus.NA_CEKANJU)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(ct);
        }
        public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId)
        {
            return await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Restaurant)
                .Include(o => o.Address)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .OrderByDescending(o => o.CreatedAt) 
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Order>> GetPendingForOwnerAsync(int ownerId, CancellationToken ct = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .Where(o =>
                    o.Status == OrderStatus.NA_CEKANJU &&
                    o.Restaurant.OwnerId == ownerId)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(ct);
        }
    }
}

using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Enums;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly GozbaDbContext _db;
        public EmployeeRepository(GozbaDbContext db) => _db = db;

        public Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return _db.Users
                .Where(u => u.Id == id && u.Role == Role.Employee)
                .FirstOrDefaultAsync(ct);
        }

        public Task<User?> GetByIdWithRestaurantAsync(int id, CancellationToken ct = default)
        {
            return _db.Users
                .Include(u => u.EmployeeRestaurant)
                .Where(u => u.Id == id && u.Role == Role.Employee)
                .FirstOrDefaultAsync(ct);
        }
    }
}

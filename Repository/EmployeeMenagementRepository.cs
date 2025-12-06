using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class EmployeeMenagementRepository : IEmployeeMenagementRepository
    {
        private readonly GozbaDbContext _db;
        public EmployeeMenagementRepository(GozbaDbContext db) => _db = db;

        public Task<List<EmployeeMenagement>> GetByRestaurantIdAsync(int restaurantId)
        {
            return _db.EmployeeMenagements
                .Where(e => e.RestaurantId == restaurantId)
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public Task<EmployeeMenagement?> GetAsync(int id)
        {
            return _db.EmployeeMenagements.FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<EmployeeMenagement?> GetByEmailAsync(string email)
        {
            return _db.EmployeeMenagements.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<EmployeeMenagement> CreateAsync(EmployeeMenagement employee)
        {
            _db.EmployeeMenagements.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateAsync(EmployeeMenagement employee)
        {
            _db.EmployeeMenagements.Update(employee);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var e = await GetAsync(id);
            if (e == null) return;

            e.Status = (e.Status == EmployeeStatus.Active)
                ? EmployeeStatus.Suspended
                : EmployeeStatus.Active;

            await _db.SaveChangesAsync();
        }
    }
}

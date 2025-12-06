using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Enums;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Service.Implementations
{
    public class EmployeeOrderService : IEmployeeOrderService
    {
        private readonly IEmployeeRepository _employees;
        private readonly GozbaDbContext _db;

        public EmployeeOrderService(IEmployeeRepository employees, GozbaDbContext db)
        {
            _employees = employees;
            _db = db;
        }

        public async Task<List<EmployeeOrderListItemDto>> GetPendingOrdersForEmployeeAsync(
            int employeeUserId,
            CancellationToken ct = default)
        {
            // 1) Učitaj zaposlenog + restoran
            var employee = await _employees.GetByIdWithRestaurantAsync(employeeUserId, ct)
                ?? throw new NotFoundException("Employee", employeeUserId);

            if (employee.EmployeeRestaurantId == null)
            {
                // možeš i da vratiš praznu listu, ali ovako je jasnija greška
                throw new BadRequestException("Zaposleni nije dodeljen nijednom restoranu.");
            }

            var restaurantId = employee.EmployeeRestaurantId.Value;

            // 2) Porudžbine na čekanju za TAJ restoran
            var query = _db.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .Where(o =>
                    o.Status == OrderStatus.NA_CEKANJU &&
                    o.RestaurantId == restaurantId)
                .OrderBy(o => o.CreatedAt);

            var list = await query
                .Select(o => new EmployeeOrderListItemDto
                {
                    Id = o.Id,
                    RestaurantId = o.RestaurantId,
                    RestaurantName = o.Restaurant.Name,

                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,

                    AddressId = o.AddressId,
                    AddressText = $"{o.Address.Street} {o.Address.HouseNumber}, {o.Address.City}",

                    Total = o.Total,
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(ct);

            return list;
        }
    }
}

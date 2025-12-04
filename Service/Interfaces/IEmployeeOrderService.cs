using gozba_na_klik.Dtos;

namespace gozba_na_klik.Service.Interfaces
{
    public interface IEmployeeOrderService
    {
        Task<List<EmployeeOrderListItemDto>> GetPendingOrdersForEmployeeAsync(int empoloyeeUserId, CancellationToken ct = default(CancellationToken));
    }
}

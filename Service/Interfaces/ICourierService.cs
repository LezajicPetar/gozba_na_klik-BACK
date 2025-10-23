using gozba_na_klik.Dtos.Users;

namespace gozba_na_klik.Service.Interfaces
{
    public interface ICourierService
    {
        // Osnovne metode za rad sa kuririma
        Task EnsureCourierAsync(int userId, string? vehicleType = null, CancellationToken ct = default); Task<bool> ExistsAsync(int userId, CancellationToken ct = default);
        Task SuspendAsync(int userId, CancellationToken ct = default);
        Task UnsuspendAsync(int userId, CancellationToken ct = default);

        // Raspored (WorkTime) kurira
        Task<WeeklyScheduleResponseDto> GetScheduleAsync(int userId, CancellationToken ct = default);
        Task UpsertScheduleAsync(int userId, WeeklyScheduleUpsertRequestDto dto, CancellationToken ct = default);

        // Status kurira
        Task<CourierStatusResponseDto> GetStatusNowAsync(int userId, CancellationToken ct = default);

    }
}

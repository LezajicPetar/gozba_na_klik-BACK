using gozba_na_klik.Data;
using gozba_na_klik.Enums;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Service.Implementations
{
    public class AutoAssignOrdersService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<AutoAssignOrdersService> _log;

        public AutoAssignOrdersService(IServiceProvider sp, ILogger<AutoAssignOrdersService> log)
        {
            _sp = sp;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<GozbaDbContext>();

                    // 1. Porudzbine bez kurira AZ
                    var orders = await db.Set<Order>()
                        .Where(o => o.Status == OrderStatus.PRIHVACENA && o.CourierId == null)
                        .OrderBy(o => o.CreatedAt)
                        .Take(20)
                        .ToListAsync(ct);

                    if (orders.Count > 0)
                    {
                        // 2. Slobodni kuriri AZ
                        var nowLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Europe/Belgrade"));

                        var freeCouriersQuery = db.Users
                            .Where(u => u.Role == Role.Courier
                                        && !u.IsSuspended
                                        && u.isActive
                                        && !u.IsBusy);

                        // Ogranici na one koji su trenutno po rasporedu "Active" AZ
                        var freeCouriers = await freeCouriersQuery
                            .OrderBy(u => u.Id)
                            .ToListAsync(ct);

                        foreach (var order in orders)
                        {
                            var courier = freeCouriers.FirstOrDefault();
                            if (courier == null) break;

                            // 3. Dodeli porudzbinu kuriru i obelezi zauzet AZ
                            await using var trx = await db.Database.BeginTransactionAsync(ct);

                            // re-check u bazi da neko drugi nije uzeo porudzbinu AZ
                            var dbCourier = await db.Users.FirstAsync(u => u.Id == courier.Id, ct);
                            var dbOrder = await db.Set<Order>().FirstAsync(o => o.Id == order.Id, ct);

                            if (dbCourier.IsBusy || dbOrder.CourierId != null || dbOrder.Status != OrderStatus.PRIHVACENA)
                            {
                                await trx.RollbackAsync(ct);
                                continue;
                            }

                            dbOrder.CourierId = dbCourier.Id;
                            dbOrder.Status = OrderStatus.PREUZIMANJE_U_TOKU;
                            dbCourier.IsBusy = true;
                            dbCourier.CurrentOrderId = dbOrder.Id;

                            await db.SaveChangesAsync(ct);
                            await trx.CommitAsync(ct);

                            // Ukloni kurira iz liste slobodnih AZ
                            freeCouriers.Remove(courier);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Greska u AutoAssignOrdersService");
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10), ct);
        }
    }
}

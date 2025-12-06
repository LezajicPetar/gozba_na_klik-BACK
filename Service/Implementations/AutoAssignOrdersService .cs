using gozba_na_klik.Data;
using gozba_na_klik.Enums;
using gozba_na_klik.Model.Entities;
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

                    // Porudzbine bez kurira AZ
                    var orders = await db.Set<Order>()
                        .Where(o => o.Status == OrderStatus.PRIHVACENA && o.CourierId == null)
                        .OrderBy(o => o.CreatedAt)
                        .Take(20)
                        .ToListAsync(ct);

                    if (orders.Count > 0)
                    {
                        // Vreme sada AZ
                        var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Belgrade");
                        var nowLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz);

                        var dow = (int)nowLocal.DayOfWeek;             
                        var t = TimeOnly.FromDateTime(nowLocal);

                        // Osnovni filter: kuriri koji nisu suspendovani, aktivni su i nisu zauzeti AZ
                        var baseFreeCouriers = await db.Users
                            .Where(u => u.Role == Role.Courier
                                        && !u.IsSuspended
                                        && u.IsActive      
                                        && !u.IsBusy)     
                            .OrderBy(u => u.Id)
                            .ToListAsync(ct);

                        if (baseFreeCouriers.Count == 0)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(10), ct);
                            continue;
                        }

                        // Uzmemo njihove Id-eve AZ
                        var courierIds = baseFreeCouriers.Select(c => c.Id).ToList();

                        // Ucitamo WorkTime za danas za te kurire AZ
                        var todaySlots = await db.WorkTimes
                            .Where(w => courierIds.Contains(w.UserId) && w.DayOfWeek == dow)
                            .Select(w => new { w.UserId, w.Start, w.End })
                            .ToListAsync(ct);

                        // Kuriri koji su sada u smeni (Start <= t < End) AZ
                        var activeCourierIds = todaySlots
                            .Where(w => t >= w.Start && t < w.End)
                            .Select(w => w.UserId)
                            .Distinct()
                            .ToHashSet();

                        // Konacna lista slobodnih + u smeni AZ
                        var freeCouriers = baseFreeCouriers
                            .Where(c => activeCourierIds.Contains(c.Id))
                            .ToList();

                        if (freeCouriers.Count == 0)
                        {
                            // Nema kurira koji su trenutno u smeni AZ
                            await Task.Delay(TimeSpan.FromSeconds(10), ct);
                            continue;
                        }

                        // Dodela porudzbina kao i ranije AZ
                        foreach (var order in orders)
                        {
                            var courier = freeCouriers.FirstOrDefault();
                            if (courier == null) break;

                            await using var trx = await db.Database.BeginTransactionAsync(ct);

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

                            // ovaj je sada zauzet – uklanjamo ga iz liste slobodnih AZ
                            freeCouriers.Remove(courier);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Greska u AutoAssignOrdersService");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), ct);
            }
        }

    }
}

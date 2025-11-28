using System.Globalization;
using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Enums;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Service.Implementations
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _couriers;
        private readonly GozbaDbContext _db;

        private const double MaxDailyHours = 10.0;
        private const double MaxWeeklyHours = 40.0;

        private static readonly TimeZoneInfo Tz = ResolveTz();
        private static TimeZoneInfo ResolveTz()
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Europe/Belgrade"); }
            catch { return TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); }
        }

        public CourierService(ICourierRepository couriers, GozbaDbContext db)
        {
            _couriers = couriers;
            _db = db;
        }

        public async Task EnsureCourierAsync(int userId, string? vehicleType = null, CancellationToken ct = default)
        {
            var user = await _couriers.EnsureCourierRoleAsync(userId);
            _ = user; _ = vehicleType;
        }

        public async Task<bool> ExistsAsync(int userId, CancellationToken ct = default)
        {
            return await _couriers.ExistsByUserIdAsync(userId);
        }

        public async Task SuspendAsync(int userId, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.Courier, ct)
                          ?? throw new NotFoundException("Courier", userId);

            user.IsSuspended = true;
            await _db.SaveChangesAsync(ct);
        }

        public async Task UnsuspendAsync(int userId, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.Courier, ct)
                       ?? throw new NotFoundException("Courier", userId);

            user.IsSuspended = false;
            await _db.SaveChangesAsync(ct);
        }


        // Raspored (WorkTime) kurira AZ
        public async Task<WeeklyScheduleResponseDto> GetScheduleAsync(int userId, CancellationToken ct = default)
        {
            var exists = await _couriers.ExistsByUserIdAsync(userId);
            if (!exists)
                throw new NotFoundException("Courier", userId);

            var days = await _db.WorkTimes
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.DayOfWeek)
                .Select(w => new DaySlotDto
                {
                    DayOfWeek = w.DayOfWeek,
                    Start = w.Start.ToString("HH\\:mm"),
                    End = w.End.ToString("HH\\:mm")
                })
                .ToListAsync(ct);

            double weekly = 0;
            foreach (var d in days)
            {
                var s = TimeOnly.ParseExact(d.Start, "HH:mm", CultureInfo.InvariantCulture);
                var e = TimeOnly.ParseExact(d.End, "HH:mm", CultureInfo.InvariantCulture);
                weekly += (e.ToTimeSpan() - s.ToTimeSpan()).TotalHours;
            }

            return new WeeklyScheduleResponseDto
            {
                CourierUserId = userId,
                Days = days,
                WeeklyHours = Math.Round(weekly, 2)
            };
        }

        public async Task UpsertScheduleAsync(int userId, WeeklyScheduleUpsertRequestDto dto, CancellationToken ct = default)
        {
            await using var trx = await _db.Database.BeginTransactionAsync(ct);

            if (!await _couriers.ExistsByUserIdAsync(userId))
                throw new NotFoundException("Courier", userId);

            if (dto.Days is null || dto.Days.Count != 7)
                throw new BadRequestException("Schedule mora imati tacno 7 dana (0..6).");

            var dup = dto.Days.GroupBy(x => x.DayOfWeek).FirstOrDefault(g => g.Count() > 1);
            if (dup != null)
                throw new BadRequestException($"Duplikat za dan {dup.Key}. Dozvoljen je najvise jedan slot po danu.");

            var expected = Enumerable.Range(0, 7).ToArray();
            var actual = dto.Days.Select(x => x.DayOfWeek).OrderBy(x => x).ToArray();
            if (!expected.SequenceEqual(actual))
                throw new BadRequestException("Mora postojati po jedan zapis za svaki dan od 0 do 6.");

            double weekly = 0;
            foreach (var d in dto.Days)
            {
                if (d.DayOfWeek < 0 || d.DayOfWeek > 6)
                    throw new BadRequestException("DayOfWeek mora biti u intervalu 0..6.");

                if (!TimeOnly.TryParseExact(d.Start, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var s))
                    throw new BadRequestException($"Nevalidan format vremena za dan {d.DayOfWeek}: Start='{d.Start}' (ocekujem HH:mm).");

                if (!TimeOnly.TryParseExact(d.End, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var e))
                    throw new BadRequestException($"Nevalidan format vremena za dan {d.DayOfWeek}: End='{d.End}' (ocekujem HH:mm).");

                if (s > e)
                    throw new BadRequestException($"Start mora biti pre End za dan {d.DayOfWeek}.");

                var daily = (e.ToTimeSpan() - s.ToTimeSpan()).TotalHours;
                if (daily > MaxDailyHours)
                    throw new BadRequestException($"Dnevni broj sati ne sme biti veci od {MaxDailyHours} za dan {d.DayOfWeek}.");

                weekly += daily;
            }

            if (weekly > MaxWeeklyHours)
                throw new BadRequestException($"Nedeljni broj sati ne sme biti veci od {MaxWeeklyHours}.");

            var existing = await _db.WorkTimes.Where(w => w.UserId == userId).ToListAsync(ct);
            _db.WorkTimes.RemoveRange(existing);
            await _db.SaveChangesAsync(ct);

            foreach (var d in dto.Days.OrderBy(x => x.DayOfWeek))
            {
                var s = TimeOnly.ParseExact(d.Start, "HH:mm", CultureInfo.InvariantCulture);
                var e = TimeOnly.ParseExact(d.End, "HH:mm", CultureInfo.InvariantCulture);

                _db.WorkTimes.Add(new WorkTime
                {
                    UserId = userId,
                    DayOfWeek = d.DayOfWeek,
                    Start = s,
                    End = e
                });
            }

            await _db.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
        }

        // Status kurira AZ

        public async Task<CourierStatusResponseDto> GetStatusNowAsync(int userId, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.Courier, ct)
                          ?? throw new NotFoundException("Courier", userId);

            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Tz);

            if (user.IsSuspended)
            {
                return new CourierStatusResponseDto { Status = "Suspended", CheckedAtLocal = nowLocal };
            }

            var dow = (int)nowLocal.DayOfWeek;     // Sunday=0 (Ned=0), bez ikakvog pomeranja
            var t = TimeOnly.FromDateTime(nowLocal);

            var wt = await _db.WorkTimes
                .Where(w => w.UserId == userId && w.DayOfWeek == dow)
                .Select(w => new { w.Start, w.End })
                .FirstOrDefaultAsync(ct);

            var active = wt != null && t >= wt.Start && t < wt.End;

            return new CourierStatusResponseDto
            {
                Status = active ? "Active" : "Inactive",
                IsBusy = user.IsBusy,
                CurrentOrderId = user.CurrentOrderId,
                CheckedAtLocal = nowLocal,
            };
        }

        // Metode vezane za isporuke AZ
        public async Task StartDeliveryAsync(int userId, int orderId, CancellationToken ct = default)
        {
            await using var trx = await _db.Database.BeginTransactionAsync(ct);

            var courier = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.Courier, ct)
                ?? throw new NotFoundException("Courier", userId);

            if (courier.IsSuspended) throw new ForbiddenException("Kurir je suspendovan.");
            if (courier.IsBusy && courier.CurrentOrderId != orderId)
                throw new ConflictException("Kurir je zauzet drugom isporukom.");

            var order = await _db.Set<Order>()
                .FirstOrDefaultAsync(o => o.Id == orderId, ct)
                ?? throw new NotFoundException("Order", orderId);

            if (order.CourierId != courier.Id)
                throw new BadRequestException("Niste dodeljeni ovoj porudzbini.");

            if (order.Status != OrderStatus.PREUZIMANJE_U_TOKU && order.Status != OrderStatus.PRIHVACENA)
                throw new BadRequestException("Porudzbina nije u stanju za preuzimanje.");

            order.Status = OrderStatus.DOSTAVA_U_TOKU;
            courier.IsBusy = true;
            courier.CurrentOrderId = order.Id;

            await _db.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
        }
        public async Task FinishedDeliveryAsync(int userId, int orderId, CancellationToken ct = default)
        {
            await using var trx = await _db.Database.BeginTransactionAsync(ct);

            var courier = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == Role.Courier, ct)
                ?? throw new NotFoundException("Courier", userId);

            var order = await _db.Set<Order>()
                .FirstOrDefaultAsync(o => o.Id == orderId, ct)
                ?? throw new NotFoundException("Order", orderId);

            if (order.CourierId != courier.Id)
                throw new BadRequestException("Niste dodeljeni ovoj porudzbini.");

            if (order.Status != OrderStatus.DOSTAVA_U_TOKU)
                throw new BadRequestException("Porudzbina nije u stanju za dostave.");

            order.Status = OrderStatus.ZAVRSENA;

            // Otkljucaj kurira AZ
            courier.IsBusy = false;
            courier.CurrentOrderId = null;

            await _db.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
        }
    }
}

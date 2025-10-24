using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model;
using Microsoft.Extensions.Logging;

namespace gozba_na_klik.Service
{
    public class OwnerRestaurantService
    {
        private readonly IRestaurantRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerRestaurantService> _logger;
        private readonly IWebHostEnvironment _env;
        private static readonly string[] Allowed = new[] { "image/jpeg", "image/png" };
    
        public OwnerRestaurantService(
            
            IRestaurantRepository repo,
            IMapper mapper,
            ILogger<OwnerRestaurantService> logger,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _env = env;
        }

        public async Task<List<RestaurantSummaryDto>> GetMineAsync(int ownerId, CancellationToken ct = default)
        {
            if (ownerId <= 0) throw new BadRequestException("ownerId is required.");
            var list = await _repo.GetByOwnerAsync(ownerId);
            return list.Select(_mapper.Map<RestaurantSummaryDto>).ToList();
        }

        public async Task<RestaurantDetailsDto> GetOneAsync(int id, int ownerId, CancellationToken ct = default)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed to access this restaurant.");
            return _mapper.Map<RestaurantDetailsDto>(r);
        }

        public async Task<RestaurantDetailsDto> UpdateGeneralAsync(int id, int ownerId, RestaurantUpsertDto dto, CancellationToken ct = default)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed to modify this restaurant.");

            _mapper.Map(dto, r);
            var saved = await _repo.UpdateAsync(r);

            _logger.LogInformation("Owner {OwnerId} updated general info for restaurant {RestaurantId}", ownerId, id);
            return _mapper.Map<RestaurantDetailsDto>(saved!);
        }

        public async Task<string> UpdateCoverAsync(int id, int ownerId, IFormFile? file, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                throw new BadRequestException("File is missing.");

            if (!Allowed.Contains(file.ContentType))
                throw new BadRequestException("Only JPEG or PNG images are allowed.");

            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed to modify this restaurant.");

            var root = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "restaurants", id.ToString());
            Directory.CreateDirectory(root);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var name = $"cover_{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(root, name);

            await using (var stream = File.Create(fullPath))
                await file.CopyToAsync(stream, ct);

            // ukloni staru ako postoji
            if (!string.IsNullOrWhiteSpace(r.Photo))
            {
                var oldPath = Path.Combine(root, r.Photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(oldPath)) File.Delete(oldPath);
            }

            var relUrl = $"/uploads/restaurants/{id}/{name}";
            r.Photo = relUrl;
            await _repo.UpdateAsync(r);

            _logger.LogInformation("Owner {OwnerId} updated restaurant cover {RestaurantId}", ownerId, id);
            return relUrl;
        }

        //work time - schedule
        public async Task<List<RestaurantWorkTime>> GetScheduleAsync(int id, int ownerId)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed.");
            return await _repo.GetWorkTimesAsync(id);
        }

        public async Task SetScheduleAsync(int id, int ownerId, IEnumerable<RestaurantWorkTimeDto> times)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed.");

            var safeTimes = times.Select(t => new RestaurantWorkTime
            {
                RestaurantId = id,
                DayOfWeek = t.DayOfWeek,
                IsClosed = t.IsClosed,
                Open = TimeSpan.TryParse(t.Open, out var open) ? open : TimeSpan.Zero,
                Close = TimeSpan.TryParse(t.Close, out var close) ? close : TimeSpan.Zero
            });

            await _repo.SetWorkTimesAsync(id, safeTimes);
        }


        //exceptions - neradni dani

        public async Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int id, int ownerId)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed.");
            return await _repo.GetExceptionsAsync(id);
        }
        public async Task<RestaurantExceptionDate> AddExceptionAsync(int id, int ownerId, RestaurantExceptionDto dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed.");

            if (string.IsNullOrWhiteSpace(dto.Date))
                throw new BadRequestException("Date is required and cannot be empty.");

            DateTime parsedDate = DateTime.SpecifyKind(DateTime.Parse(dto.Date), DateTimeKind.Utc);

            var toAdd = new RestaurantExceptionDate
            {
                RestaurantId = id,
                Date = parsedDate,
                Reason = dto.Reason,
            };

            return await _repo.AddExceptionAsync(toAdd);
        }


        public async Task<bool> DeleteExceptionAsync(int id, int ownerId, int exId)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r is null) throw new NotFoundException("Restaurant", id);
            if (r.OwnerId != ownerId) throw new ForbiddenException("Not allowed.");
            return await _repo.DeleteExceptionAsync(exId);
        }
    }
}

﻿using gozba_na_klik.Dtos.Restaurants;

namespace gozba_na_klik.Service
{
    public interface IOwnerRestaurantService
    {
        Task<List<RestaurantSummaryDto>> GetMineAsync(int ownerId, CancellationToken ct = default);
        Task<RestaurantDetailsDto> GetOneAsync(int id, int ownerId, CancellationToken ct = default);
        Task<RestaurantDetailsDto> UpdateGeneralAsync(int id, int ownerId, RestaurantUpsertDto dto, CancellationToken ct = default);
        Task<string> UpdateCoverAsync(int id, int ownerId, IFormFile? file, CancellationToken ct = default);

        // schedule (radno vreme)
        Task<List<RestaurantWorkTimeDto>> GetScheduleAsync(int id, int ownerId);
        Task SetScheduleAsync(int id, int ownerId, IEnumerable<RestaurantWorkTimeDto> times);

        // exception dates (neradni dani)
        Task<List<RestaurantExceptionDto>> GetExceptionsAsync(int id, int ownerId);
        Task<RestaurantExceptionDto> AddExceptionAsync(int id, int ownerId, RestaurantExceptionDto dto);
        Task<bool> DeleteExceptionAsync(int id, int ownerId, int exId);
    }
}

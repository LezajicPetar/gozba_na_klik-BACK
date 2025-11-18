using gozba_na_klik.Dtos.Review;

namespace gozba_na_klik.Service
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateAsync(CreateReviewDto dto);
    }
}

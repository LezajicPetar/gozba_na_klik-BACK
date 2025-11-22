using AutoMapper;
using gozba_na_klik.Dtos.Review;
using gozba_na_klik.Model;

namespace gozba_na_klik.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            var review = _mapper.Map<Review>(dto);

            var created = await _reviewRepo.CreateAsync(review);

            var updated = await _reviewRepo.GetByIdAsync(created.Id);

            return _mapper.Map<ReviewDto>(updated);
        }
    }
}

using gozba_na_klik.Dtos.Review;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik._Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/reviews")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<ReviewDto>> CreateAsync([FromBody]CreateReviewDto dto)
        {
            var created = await _reviewService.CreateAsync(dto);

            return Ok(created);
        }
    }
}

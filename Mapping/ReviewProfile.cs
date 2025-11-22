using AutoMapper;
using gozba_na_klik.Dtos.Review;
using gozba_na_klik.Model;

namespace gozba_na_klik.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, CreateReviewDto>().ReverseMap();
            CreateMap<Review, ReviewDto>()
                .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();
        }
    }
}

using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Repositories.Review;
using LocatePlate.Service.Abstract;
using System.Collections.Generic;

namespace LocatePlate.Service.Services.Review
{
    public class ReviewService : BaseService<Reviews, IReviewRepository>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
            : base(reviewRepository)
        {
            this._reviewRepository = reviewRepository;
        }

        public List<RatingGroups> GetAllByRestaurantId(int id) => this._reviewRepository.GetAllByRestaurantId(id);
    }
}

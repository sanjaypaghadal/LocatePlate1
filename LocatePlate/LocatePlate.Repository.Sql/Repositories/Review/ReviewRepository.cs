using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Repositories.Review
{
    public class ReviewRepository : BaseRepository<Reviews>, IReviewRepository
    {
        private readonly LocatePlateContext _locatePlateContext;
        public ReviewRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public List<RatingGroups> GetAllByRestaurantId(int id)
        {

            var result = (from review in this._locatePlateContext.Reviews
                          join rating in this._locatePlateContext.Ratings on review.RestaurantId equals rating.RestaurantId into gj
                          from subpet in gj
                          where review.RestaurantId == id
                          select new ReviewRating { Reviews = review, Ratings = subpet }
                         ).ToList();


            var ratings = result.GroupBy(c => c.Ratings.RestaurantId).Select(group =>
                                                                          new RatingGroups
                                                                          {
                                                                              Key = group.Key.ToString(),
                                                                              Avg = group.Average(c => c.Ratings.Rating),
                                                                              Reviews = group.FirstOrDefault().Reviews
                                                                          }).OrderBy(x => x.Key).ToList();


            return ratings;
        }
    }
}

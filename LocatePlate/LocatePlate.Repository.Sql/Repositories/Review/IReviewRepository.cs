using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System.Collections.Generic;

namespace LocatePlate.Repository.Repositories.Review
{
    public interface IReviewRepository : IBaseRepository<Reviews>
    {
        List<RatingGroups> GetAllByRestaurantId(int Id);
    }
}

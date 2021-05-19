using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System.Collections.Generic;

namespace LocatePlate.Service.Services.Review
{
    public interface IReviewService:IBaseService<Reviews>
    {
        public List<RatingGroups> GetAllByRestaurantId(int id);
    }
}

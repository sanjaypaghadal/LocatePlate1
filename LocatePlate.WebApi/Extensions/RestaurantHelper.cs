using LocatePlate.Model.RestaurantDomain;

namespace LocatePlate.WebApi.Extensions
{
    public class RestaurantExtend : Restaurant
    {
        public string MenuImagesToDelete { get; set; }
        public string CoverImagesToDelete { get; set; }
    }
}

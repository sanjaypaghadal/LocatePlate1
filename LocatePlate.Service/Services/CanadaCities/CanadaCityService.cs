using LocatePlate.Model.Location;
using LocatePlate.Repository.CanadaCities;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.CanadaCities
{
    public class CanadaCityService : BaseService<CanadaCity, ICanadaCityRepository>, ICanadaCityService
    {
        private readonly ICanadaCityRepository _capacityRepository;
        public CanadaCityService(ICanadaCityRepository capacityRepository) : base(capacityRepository)
        {
        }
    }
}

using LocatePlate.Model.Location;
using LocatePlate.Repository.Context;

namespace LocatePlate.Repository.CanadaCities
{
    public class CanadaCityRepository : BaseRepository<CanadaCity>, ICanadaCityRepository
    {
        public CanadaCityRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
        }
    }
}

using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Cms;
using System.Collections.Generic;

namespace LocatePlate.Repository.Pages
{
    public interface IPageRepositoryMongoDB : IBaseRepositoryMongoDB<Page>
    {
        Page GetByUrl(string url);
        List<Page> GetByCountryAndProvince(int countryId, int provinceId);
    }
}

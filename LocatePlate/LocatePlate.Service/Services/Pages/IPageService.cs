using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Cms;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Pages
{
    public interface IPageService : IBaseServiceMongoDB<Page>
    {
        Page GetByUrl(string url);
        List<Page> GetByCountryAndProvince(int countryId, int provinceId);
        Page GetFullPageByUrl(string url);
        Page GetFullPageById(Guid Id);
    }
}

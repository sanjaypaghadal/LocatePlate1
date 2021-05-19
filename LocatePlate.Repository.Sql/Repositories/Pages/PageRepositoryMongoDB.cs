using LocatePlate.Model.Cms;
using LocatePlate.Repository.Repositories.Abstract;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Pages
{
    public class PageRepositoryMongoDB : BaseRepositoryMongoDB<Page>, IPageRepositoryMongoDB
    {
        private readonly ILocatePlateMongoDBContext<Page> _locatePlateMongoDBContext;
        public PageRepositoryMongoDB(ILocatePlateMongoDBContext<Page> locatePlateMongoDBContext)
            : base(locatePlateMongoDBContext)
        {
            this._locatePlateMongoDBContext = locatePlateMongoDBContext;
        }

        public Page GetByUrl(string url)
        {
            var filter = Builders<Page>.Filter.Where(c => c.Url == url);
            return this._locatePlateMongoContext.ReadOnly(filter)?.SingleOrDefault();
        }

        public List<Page> GetByCountryAndProvince(int countryId, int provinceId)
        {
            var filter = Builders<Page>.Filter.Where(c => c.Country == countryId && c.Province == provinceId);
            return this._locatePlateMongoContext.ReadOnly(filter)?.ToList();
        }
    }
}

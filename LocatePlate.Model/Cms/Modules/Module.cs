using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Cms.Modules.Advertisement;
using LocatePlate.Model.Cms.Modules.Deals;
using LocatePlate.Model.Cms.Modules.Restaurants;
using LocatePlate.Model.Cms.Modules.SearchHeader;
using LocatePlate.Model.Cms.Modules.WALF;
using MongoDB.Bson.Serialization.Attributes;

namespace LocatePlate.Model.Cms.Modules
{
    [BsonIgnoreExtraElements]
    public class Module : BaseEntityMongoDB
    {
        public DealModule DealModule { get; set; } = null;
        public WALFModule WALFModule { get; set; } = null;
        public CardsListingModule CardsVerticalModule { get; set; } = null;
        //public CardsListingModule CardsHoritizontalModule { get; set; } = null;
        public AdvertisementModule AdvertisementModule { get; set; } = null;
        public SearchButtonsModule SearchButtonsModule { get; set; } = null;
    }
}

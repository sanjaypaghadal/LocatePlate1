using LocatePlate.Model.Cms;
using LocatePlate.Repository.Pages;
using LocatePlate.Service.Abstract;
using LocatePlate.Service.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Service.Pages
{
    public class PageService : BaseServiceMongoDB<Page, IPageRepositoryMongoDB>, IPageService
    {
        private readonly IPageRepositoryMongoDB _pageRepository;
        private readonly IRestaurantService _restaurantService;

        public PageService(IPageRepositoryMongoDB pageRepository, IRestaurantService restaurantService)
            : base(pageRepository)
        {
            this._pageRepository = pageRepository;
            this._restaurantService = restaurantService;
        }

        public Page GetByUrl(string url) => this._pageRepository.GetByUrl(url);

        public Page GetFullPageByUrl(string url)
        {
            var page = this._pageRepository.GetByUrl(url);
            page = FillModules(page);

            return page;
        }

        public Page GetFullPageById(Guid Id)
        {
            var page = this._pageRepository.Get(Id);
            page = FillModules(page);
            return page;
        }

        public List<Page> GetByCountryAndProvince(int countryId, int provinceId) => this._pageRepository.GetByCountryAndProvince(countryId, provinceId);

        private Page FillModules(Page page)
        {
            if (page?.PageLayout?.Section != null)
            {
                foreach (var item in page.PageLayout.Section)
                {
                    if (item?.Module?.CardsVerticalModule != null)
                    {
                        var ids = new List<int>();
                        var restCount = item.Module.CardsVerticalModule.Restaurants.Count;
                        foreach (var rest in item?.Module?.CardsVerticalModule.Restaurants)
                            ids.Add(rest.Id);
                        item.Module.CardsVerticalModule.RestaurantDetails = this._restaurantService.GetAllByIds(ids).ToList();
                    }
                    if (item?.Module?.DealModule != null)
                    {
                        foreach (var deal in item.Module.DealModule.Deals)
                        {
                            deal.Images = string.IsNullOrEmpty(deal.Images) ? string.Empty : $"/cms/{page.Url.ToLower()}/{deal.Images.ToLower()}";
                        }
                    }
                }

                if (page.IsNewRestaurantShow)
                    page.NewRestaurants = this._restaurantService.GetNewRestaurants(6, page.Id);
            }
            return page;
        }
    }
}

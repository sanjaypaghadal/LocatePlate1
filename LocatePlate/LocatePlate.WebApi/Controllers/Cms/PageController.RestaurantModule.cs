using LocatePlate.Infrastructure.Constant;
using LocatePlate.Model.Cms.Modules.Restaurants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocatePlate.WebApi.Controllers.Cms
{
    public partial class PageController
    {
        [HttpGet("RestaurantModulesRestaurantSelection/{prefix}/{pageId}/{pageindex}/{pagesize}")]
        public IActionResult RestaurantModulesRestaurantSelection(string prefix, Guid pageId, int pageindex, int pagesize)
        {
            ViewBag.InnerPrefix = prefix;
            var restaurants = _restaurantService.GetByLocation(pageindex, pagesize, pageId);

            return PartialView("_RestaurantListing", restaurants);
        }

        public string GenerateRestaurantMarkup(CardsListingModule module, string url)
        {
            //Get all the restaurant Ids
            var ids = new List<int>();
            var restCount = module.Restaurants.Count;
            foreach (var item in module.Restaurants)
                ids.Add(item.Id);

            // Get all the restaurants
            var restaurants = _restaurantService.GetAllByIds(ids).ToList();
            var viewAll = @" <div><a href = '#' class='hide-500 bold-500'>See More</a></div>";
            var restaurantRating = @" <div class='rating'>
                                            <span class='fa fa-star checked'></span>
                                            <span class='fa fa-star checked'></span>
                                            <span class='fa fa-star checked'></span>
                                            <span class='fa fa-star-half-o checked'></span>
                                            <span class='fa fa-star unchecked'></span>
                                        </div>";

            var restaurantReviews = @" <span class='font-12 text-grey'>100 Reviews</span>";


            var Restaurant = @$"<div class='swiper-slide'>
                        <div class='card-desktop'>
                            <a href = '{ModuleTokens.RestaurantUrl}' >
                                <div class='card-img'>
                                    <div class='position-relative'>
                                        <img class='card-img-top' src='{ModuleTokens.RestaurantImage}' alt='Card image cap'>
                                    </div>
                                </div>
                                <div class='card-body py-2 px-3 text-left'>
                                    <h5 class='card-title mb-0 font-18'>{ModuleTokens.RestaurantName}</h5>
                                    <div class='d-flex align-items-center'>
                                       {ModuleTokens.RestaurantRating}
                                       {ModuleTokens.RestaurantReviews}
                                    </div>
                                    <div class='font-12 text-grey d-flex mt-2'>
                                        <div>
                                            <i class='fa fa-cutlery mr-2'></i>
                                        </div>
                                        <div>
                                          {ModuleTokens.RestaurantCusines}
                                        </div>
                                    </div>
                                    <div class='card-text font-12 text-grey d-flex'>
                                        <div>
                                            <i class='fa fa-map-marker mr-10'></i>
                                        </div>
                                        <div>
                                           {ModuleTokens.RestaurantLocality}
                                       </div>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </div>";
            // Map restaurant with Markup
            var markup = @$"<section class='container my-5'>
                                <div>
                                  <div class='d-flex justify-content-between align-items-baseline all-restaurant-heading pb-3'>
                                     <h1 class='mobile-heading'> {ModuleTokens.ModuleName}</h1>
                                         {ModuleTokens.IsModuleViewAll}
                                  </div>
                                <div class='card-restaurants'>
                                    <div class='swiper-container'>
                                        <div class='swiper-wrapper'>
                                           {ModuleTokens.Restaurants}
                                        </div>
                                    </div>
                                </div>
                                {ModuleTokens.RestaurantsMobile}
                             </div>
                      </section>";

            var ViewAllRestaurant = @"<div>
                <a href='#' class='hide-500 bold-500 font-18'>
                    See More
                </a>
            </div>";
            var mobileRetaurant = @$"<div class='col-md-3'>
                    <div class='card'>
                        <div class='card-img'>
                            <div class='position-relative'>
                                <a href='{ModuleTokens.RestaurantUrl}'>
                                    <img class='card-img-top' src='{ModuleTokens.RestaurantImage}' alt='Card image cap'>
                                </a>
                            </div>
                        </div>
                        <div class='card-body p-0 pl-2'>
                            <a href='#'>
                                <h5 class='card-title mb-0 font-16'>{ModuleTokens.RestaurantName}</h5>
                            </a>
                            <div class='d-flex align-items-center'>
                                  {ModuleTokens.RestaurantRating}
                                  {ModuleTokens.RestaurantReviews}
                            </div>
                            <div class='font-12 text-grey d-flex mt-2'>
                                <div>
                                    <i class='fa fa-cutlery mr-2'></i>
                                </div>
                                <div>
                                    {ModuleTokens.RestaurantCusines}
                                </div>
                            </div>
                            <div class='card-text font-12 text-grey d-flex'>
                                <div>
                                    <i class='fa fa-map-marker mr-10'></i>
                                </div>
                                <div>
                                      {ModuleTokens.RestaurantLocality}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>";

            var mobileMarkup = @$"<div class='card-restaurants-mobile'>
                                  <div class='row'>
                                    {ModuleTokens.RestaurantsMobile}
                                    {ModuleTokens.IsModuleViewAll}
                                 </div>
                              </div>";

            // Fill Restaurants
            if (restCount <= 5) { viewAll = string.Empty; ViewAllRestaurant = string.Empty; }
            var RestaurantHolder = new StringBuilder();
            var RestaurantMobileHolder = new StringBuilder();

            foreach (var item in restaurants)
            {

                var coverImages = !string.IsNullOrEmpty(item.CoverImages) && item.CoverImages.Contains(',') ? item.CoverImages.Split(',')[0]:string.Empty;

                RestaurantHolder.Append(Restaurant.Replace(ModuleTokens.RestaurantUrl, $"{ModuleTokens.RestaurantHost}/{url}/restaurant/{item.Url.ToLower()}")
                            .Replace(ModuleTokens.RestaurantName, item.Name)
                            .Replace(ModuleTokens.RestaurantImage, $"/UploadImages/{item.UserId}/{item.Name}/{coverImages}")
                            .Replace(ModuleTokens.RestaurantRating, restaurantRating)
                            .Replace(ModuleTokens.RestaurantReviews, restaurantReviews)
                            .Replace(ModuleTokens.RestaurantCusines, item.Cuisine)
                            .Replace(ModuleTokens.RestaurantLocality, item.Locality));

                RestaurantMobileHolder.Append(mobileRetaurant.Replace(ModuleTokens.RestaurantUrl, $"{ModuleTokens.RestaurantHost}/{url}/restaurant/{item.Url.ToLower()}")
                       .Replace(ModuleTokens.RestaurantName, item.Name)
                       .Replace(ModuleTokens.RestaurantImage, $"/UploadImages/{item.UserId}/{item.Name}/{coverImages}")
                       .Replace(ModuleTokens.RestaurantRating, restaurantRating)
                       .Replace(ModuleTokens.RestaurantReviews, restaurantReviews)
                       .Replace(ModuleTokens.RestaurantCusines, item.Cuisine)
                       .Replace(ModuleTokens.RestaurantLocality, item.Locality));
            }

            mobileMarkup = mobileMarkup.Replace(ModuleTokens.RestaurantsMobile, RestaurantMobileHolder.ToString())
                .Replace(ModuleTokens.IsModuleViewAll, ViewAllRestaurant);

            markup = markup.Replace(ModuleTokens.ModuleName, module.Name)
                 .Replace(ModuleTokens.IsModuleViewAll, viewAll)
                 .Replace(ModuleTokens.Restaurants, RestaurantHolder.ToString())
                 .Replace(ModuleTokens.RestaurantsMobile, mobileMarkup);



            return markup;
        }
    }
}

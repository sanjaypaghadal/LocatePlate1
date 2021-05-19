using LocatePlate.Model.Cms.Modules;
using LocatePlate.Service.Services.Modules;
using LocatePlate.WebApi.Contracts.CmsRoutes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Cms
{
    [Route(CmsRoute.BaseRoute.Cms)]
    public class SeedModuleController : BaseController
    {
        private readonly IModuleService _moduleService;
        public SeedModuleController(IModuleService moduleService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._moduleService = moduleService;
        }

        public IActionResult Index()
        {
            this._moduleService.DeleteAll();
            var module = new Module
            {
                DealModule = new Model.Cms.Modules.Deals.DealModule
                {
                    Markup = @"<section class='box-shadows top-offers-section'>
                                <div class='swiper-container'>
                                    <div class='swiper-wrapper'>
                                        <div class='swiper-slide'><img class='d-block w-100' src='/images/carousel/1.png' alt='First slide'></div>
                                    </div>
                                    <div class='swiper-pagination mobile-slider'></div>
                                    <!-- Add Arrows -->
                                    <div class='swiper-button-next desktop-slider'></div>
                                    <div class='swiper-button-prev desktop-slider'></div>
                                </div>
                            </section>",
                    Name = "Slider",
                },
                AdvertisementModule = new Model.Cms.Modules.Advertisement.AdvertisementModule
                {
                    Name = "Advertisement",
                    Markup = @"<section class='container my-5'>
                                    <div class='best-deals-section'>
                                        <h4>Best Deals</h4>
                                        <div class='w-100 d-flex flex-wrap'>
                                            <div class='flex-box'>
                                                <div class='d-flex'>
                                                    <div class='best-deals-img'>
                                                        <img src='/images/restaurant/2.jpg' />
                                                    </div>
                                                    <div class='padding-10 best-deals-text'>
                                                        <span class='font-16 bold-600'>Prime deals</span>
                                                        <p class='mobile-hide'>Minimum 25% off at the hottest and most premium restaurants</p>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class='flex-box'>
                                                <div class='d-flex'>
                                                    <div class='best-deals-img'>
                                                        <img src='/images/restaurant/3.jpg' />
                                                    </div>
                                                    <div class='padding-10 best-deals-text'>
                                                        <span class='font-16 bold-600'>Fast Food</span>
                                                        <p class='mobile-hide'>Discover all Quick Bites food places close to you</p>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='flex-box'>
                                                <div class='d-flex'>
                                                    <div class='best-deals-img'>
                                                        <img src='/images/restaurant/4.jpg' />
                                                    </div>
                                                    <div class='padding-10 best-deals-text'>
                                                        <span class='font-16 bold-600 text-nowrap'>Prepaid deals</span>
                                                        <p class='mobile-hide'>Hassle Free meals with great value!</p>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </section>"
                },
                WALFModule = new Model.Cms.Modules.WALF.WALFModule
                {
                    Name = "Walf",
                    Markup = @"
                                <section class='my-5'>
                                    <div class='container head-section'>
                                        <div class='row'>
                                            <div class='col-md-3 col-sm-6 col-xs-12'>
                                                <div class='looking-section'>
                                                    <a href='#' class='image-round ml-2 mr-3'>
                                                        <img src='/images/website/map.png' height='180' />
                                                    </a>
                                                    <div>
                                                        <h5 class='text-wrap'>Near Me</h5>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-md-3 col-sm-6 col-xs-12'>
                                                <div class='looking-section'>
                                                    <a href='#' class='image-round ml-2 mr-3'>
                                                        <img src='/images/website/cook_breakfast.png' />
                                                    </a>
                                                    <div>
                                                        <h5>Breakfast</h5>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-md-3 col-sm-6 col-xs-12'>
                                                <div class='looking-section'>
                                                    <a href='#' class='image-round ml-2 mr-3'>
                                                        <img src='/images/website/cook_lunch.png' />
                                                    </a>
                                                    <div>
                                                        <h5>Lunch</h5>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-md-3 col-sm-6 col-xs-12'>
                                                <div class='looking-section'>
                                                    <a href='#' class='image-round ml-2 mr-3'>
                                                        <img src='/images/website/cook_dinner.png' />
                                                    </a>
                                                    <div>
                                                        <h5>Dinner</h5>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>"
                },
                CardsVerticalModule = new Model.Cms.Modules.Restaurants.CardsListingModule
                {
                    Name = "Restaurant",
                    Markup = @"<div class='d-flex justify-content-between pb-3 align-items-baseline'>
                                <h3>Restaurants</h3>
                                <div>
                                    <a href='#' class='hide-500'>
                                        View all
                                    </a>
                                </div>
                            </div>
                            <div class='card-restaurants'>
                                <div class='swiper-container'>
                                    <div class='swiper-wrapper'>
                                        <div class='swiper-slide'>
                                            <div class='card-desktop'>
                                                <a href='#'>
                                                    <div class='card-img'>
                                                        <div class='position-relative'>
                                                            <img class='card-img-top' src='~/images/restaurant/2.jpg' alt='Card image cap'>
                                                        </div>
                                                    </div>
                                                    <div class='card-body py-2 px-3 text-left'>
                                                        <h5 class='card-title mb-0 font-18'>The Immigrant Cafe</h5>
                                                        <small class='card-text mb-0 font-14'>Connaught Place (CP), Central Delhi</small>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                        <div class='swiper-slide'>
                                            <div class='card-desktop'>
                                                <a href='#'>
                                                    <div class='card-img'>
                                                        <div class='position-relative'>
                                                            <img class='card-img-top' src='~/images/restaurant/3.jpg' alt='Card image cap'>
                                                        </div>
                                                    </div>
                                                    <div class='card-body py-2 px-3 text-left'>
                                                        <h5 class='card-title mb-0 font-18'>Gourmet Couch</h5>
                                                        <small class='card-text mb-0 font-14'>Connaught Place (CP), Central Delhi</small>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                        <div class='swiper-slide'>
                                            <div class='card-desktop'>
                                                <a href='#'>
                                                    <div class='card-img'>
                                                        <div class='position-relative'>
                                                            <img class='card-img-top' src='~/images/restaurant/10.png' alt='Card image cap'>
                                                        </div>
                                                    </div>
                                                    <div class='card-body py-2 px-3 text-left'>
                                                        <h5 class='card-title mb-0 font-18'>United Coffee House</h5>
                                                        <small class='card-text mb-0 font-14'>Connaught Place (CP), Central Delhi</small>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                        <div class='swiper-slide'>
                                            <div class='card-desktop'>
                                                <a href='#'>
                                                    <div class='card-img'>
                                                        <div class='position-relative'>
                                                            <img class='card-img-top' src='~/images/restaurant/3.jpg' alt='Card image cap'>
                                                        </div>
                                                    </div>
                                                    <div class='card-body py-2 px-3 text-left'>
                                                        <h5 class='card-title mb-0 font-18'>Gourmet Couch</h5>
                                                        <small class='card-text mb-0 font-14'>Connaught Place (CP), Central Delhi</small>
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Add Arrows -->
                                    <div class='swiper-button-next desktop-slider'></div>
                                    <div class='swiper-button-prev desktop-slider'></div>
                                </div>
                            </div>"
                }
            };
            this._moduleService.Save(module);
            return RedirectToRoute(new
            {
                controller = "Page",
                action = "Index",
            });
        }
    }
}


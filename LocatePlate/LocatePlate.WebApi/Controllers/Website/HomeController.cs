using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.Capactities;
using LocatePlate.Service.Menus;
using LocatePlate.Service.Pages;
using LocatePlate.Service.Restaurants;
using LocatePlate.Service.Services.Discounts;
using LocatePlate.Service.Services.Review;
using LocatePlate.Service.Services.SearchManager;
using LocatePlate.Service.Services.Users;
using LocatePlate.Service.Timings;
using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LocatePlate.WebApi.Controllers.Website
{
    //[Authorize]
    public partial class HomeController : BaseController
    {
        private readonly IPageService _pageService;
        private readonly IRestaurantService _restaurantService;
        private readonly IBookingService _bookingService;
        private readonly ITimingService _timingService;
        private readonly ICapacityService _capacityService;
        private readonly ISearchService _searchService;
        private readonly IClientSide _ClientSide;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMenuService _menuService;
        private readonly IDiscountService _discountService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public HomeController(IPageService pageService, IBookingService bookingService, ICapacityService capacityService,
            ITimingService timingService, IRestaurantService restaurantService, ISearchService searchService,
            IClientSide clientSide, IMenuService menuService, IDiscountService discountService, IReviewService reviewService, IUserService userService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._pageService = pageService;
            this._restaurantService = restaurantService;
            this._bookingService = bookingService;
            this._timingService = timingService;
            this._capacityService = capacityService;
            this._searchService = searchService;
            this._ClientSide = clientSide;
            this._contextAccessor = contextAccessor;
            this._menuService = menuService;
            this._discountService = discountService;
            this._reviewService = reviewService;
            this._userService = userService;
        }

        [HttpGet("", Order = 1)]
        [HttpGet("home", Order = 2)]

        public IActionResult Index()
        {
            string cityname = "";
            getCityFromCookie(ref cityname);
            if (!string.IsNullOrEmpty(cityname))
                return RedirectToRoute("locationhome", new { url = cityname.ToLower() });
            else
                return RedirectToRoute("locationhome", new { url = LocationConstant.DefaultLocation }); // default city
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet("errorpage")]
        public IActionResult ErrorPage()
        {
            return View("ErrorPage");
        }

        [HttpGet]
        public IActionResult CancelToOrderValidation(int OrderId, string billid)
        {
            try
            {
                var bookingResult = this._bookingService.Get(OrderId);
                var restaurantResult = this._restaurantService.Get(bookingResult.RestaurantId);
                bookingResult.ReservationModel = new Reservation() { RestaurantName = restaurantResult.Name };
                var cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    IsEssential = true,
                    HttpOnly = false,
                    Secure = true,
                };

                string bookingJson = JsonSerializer.Serialize(bookingResult);

                this._contextAccessor.HttpContext.Response.Cookies.Append($"booking{bookingResult.RestaurantId}", bookingJson, cookieOptions);

                return View("~/Views/Booking/ConfirmOrder.cshtml", bookingResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult OrderValidation(Booking booking)
        {
            try
            {
                booking.StartTimeDisplay = booking.StartTime.ToString("HH:mm:ss");
                ModelState.Remove("ReservationType");
                if (ModelState.IsValid)
                {
                    var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);
                    booking.ReservationType = (Convert.ToInt32($"{dict["ReservationType"].FirstOrDefault()}") == 0) ? false : true;
                    booking.UserId = UserId;
                    booking.CreatedDate = booking.ModifiedDate = DateTime.UtcNow;
                    booking.CreatedBy = booking.ModifiedBy = User.Identity.Name;
                    //var dateTimeUnspec = DateTime.SpecifyKind(booking.StartTime, DateTimeKind.Unspecified);
                    //booking.StartTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._ClientSide.ClientInfo);
                    booking.BillId = Guid.NewGuid().ToString().Replace('-', ' ').ToLower();
                    if (booking?.MenuItems != null)
                        foreach (var item in booking?.MenuItems)
                            item.RestaurantId = booking.RestaurantId;
                    // this._contextAccessor.HttpContext.Response.Cookies.Append($"order{Booking.RestaurantId}", "");

                    var bookingResult = this._bookingService.InsertBooking(booking);
                    //var bookingResult = this._bookingService.Insert(booking);

                    FCMHelper.SendPushNotification("LocatePlate", "New order", "there is new order", UserId, bookingResult.RestaurantId, 1);
                    var cookieOptions = new CookieOptions()
                    {
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        IsEssential = true,
                        HttpOnly = false,
                        Secure = true,
                    };

                    string bookingJson = JsonSerializer.Serialize(booking);

                    this._contextAccessor.HttpContext.Response.Cookies.Append($"booking{booking.RestaurantId}", bookingJson, cookieOptions);

                    return View("~/Views/Booking/ConfirmOrder.cshtml", booking);
                }
                else
                {
                    var errors = new StringBuilder();
                    ModelState.Values.SelectMany(v => v.Errors.Select(d => errors.Append(d.ErrorMessage)));
                    // If we got this far, something failed, redisplay form
                    return StatusCode(500, errors.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("confirmingpage")]
        public IActionResult ConfirmingPage()
        {
            return View("ConfirmingPage");
        }
        [HttpGet("paymentcancel")]
        public IActionResult PaymentCancel()
        {
            return View("PaymentCancel");
        }
        [HttpGet("loadingpage")]
        public IActionResult LoadingPage()
        {
            return View("LoadingPage");
        }
        [HttpGet("restaurant/{restaurantId}", Name = "RestaurantDetailId", Order = 1)]
        [HttpGet("{url}/restaurant/{restaurantUrl}", Name = "RestaurantDetail", Order = 2)]

        public IActionResult RestaurantDetail(string url, string restaurantUrl, int restaurantId = 0)
        {
            var restaurant = new Restaurant();
            if (restaurantId != 0)
                restaurant = this._restaurantService.GetDetailById(restaurantId);
            else
                restaurant = this._restaurantService.GetDetailByUrl(restaurantUrl, url);

            ViewData["Title"] = $"{restaurant.Name} | LocatePlate";
            TempData["LocationId"] = restaurant.LocationId.ToString();
            restaurant.Url = restaurantUrl;
            restaurant.CityName = url;
            GetBookingInfo(restaurant.Id);
            GetOrder(ref restaurant);
            restaurant.ReviewRating = GetReview(restaurant.Id);

            foreach (var item in restaurant.Timings)
            {
                item.IsOpen = true;
                item.ClientInfo = _ClientSide.ClientInfo;
            }
            var defaultmodel = PrepareAllDayTime();
            ICollection<Timing> newtiming = (from dd in defaultmodel
                                             join m in restaurant.Timings on dd.Day equals m.Day into g
                                             from result in g.DefaultIfEmpty()
                                             select (result == null ? dd : result)).ToList();
            restaurant.Timings = newtiming;

            return View(restaurant);
        }

        private IEnumerable<Timing> PrepareAllDayTime()
        {
            return new List<Timing>(){
             new Timing { IsOpen = false, Day = DayOfWeek.Monday },
             new Timing { IsOpen = false, Day = DayOfWeek.Tuesday },
             new Timing { IsOpen = false, Day = DayOfWeek.Wednesday },
             new Timing { IsOpen = false, Day = DayOfWeek.Thursday },
             new Timing { IsOpen = false, Day = DayOfWeek.Friday },
             new Timing { IsOpen = false, Day = DayOfWeek.Saturday },
             new Timing { IsOpen = false, Day = DayOfWeek.Sunday }
            };
        }

        [HttpGet("{url}/restaurants", Name = "locationhome")]
        public IActionResult Page(string url)
        {
            var page = this._pageService.GetFullPageByUrl($"{url}");
            if (page == null)
                return RedirectToRoute("locationhome", new { url = LocationConstant.DefaultLocation }); // default city

            setlocationInCookie(page.Id, page.Name);
            setSetUserLocationInCookie(page.Logitude, page.Latitude);
            ///if (page == null) return RedirectToAction(nameof(Index));
            ViewData["Meta"] = page.MetaData;
            return View("Index", page);
        }

        [HttpGet("SetUserLocation/{logitude}/{latitude}")]

        public IActionResult SetUserLocation(decimal logitude, decimal latitude)
        {

            var searchQuery = new SearchQuery { Latitude = logitude, Logitude = latitude };
            var currentLocation = this._searchService.GetNearByLocation(searchQuery);
            if (currentLocation != null & currentLocation?.LocationId != null && currentLocation?.LocationId != Guid.Empty)
            {
                setlocationInCookie(currentLocation.LocationId, currentLocation.CityName);
                setSetUserLocationInCookie(logitude, latitude);
            }

            return Ok(true);
        }


        [HttpGet("rating/{rate}/{restaurantId}/{ratingType}")]
        public IActionResult rating(int rate, int restaurantId, RatingType ratingType)
        {
            if (UserId != Guid.Empty)
            {
                var result = this._restaurantService.GiveRating(new Ratings { Rating = rate, RestaurantId = restaurantId, RatingType = ratingType, UserId = UserId });
                return Ok(result);
            }
            return Ok();
        }


        [HttpPost("review/restaurant")]
        public IActionResult review(Reviews model)
        {
            if (UserId != Guid.Empty)
            {
                var existingReview = this._reviewService.GetAllByRestaurantId(model.RestaurantId);
                model.UserId = UserId;
                model.ModifiedDate = DateTime.UtcNow;
                model.ModifiedBy = this._userService.GetUserById(UserId).BusinessName;
                Reviews review;
                if (existingReview?.Any() != null && existingReview?.FirstOrDefault()?.Reviews != null)
                {
                    existingReview.FirstOrDefault().Reviews.Review = model.Review;
                    existingReview.FirstOrDefault().Reviews.ModifiedDate = model.ModifiedDate;
                    existingReview.FirstOrDefault().Reviews.ModifiedBy = model.ModifiedBy;

                    review = this._reviewService.Update(existingReview.FirstOrDefault().Reviews);
                }
                else
                {
                    model.CreatedBy = model.ModifiedBy;
                    model.CreatedDate = model.ModifiedDate;
                    review = this._reviewService.Insert(model);
                }
                var restaurant = new Restaurant();
                restaurant.ReviewRating = GetReview(model.RestaurantId);
                return View("_AllReviews", restaurant);
            }
            return Ok();
        }



        private void SetCityName(ref SearchRecords searchResult)
        {
            var cityName = string.Empty;
            if (string.IsNullOrEmpty(searchResult.CityName))
            {
                getCityFromCookie(ref cityName);
                searchResult.CityName = cityName;
            }
        }

        private void getCityFromCookie(ref string cityName) => this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("LocationName", out cityName);
        private void getLocationIdFromCookie(ref string LocationId) => this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("LocationId", out LocationId);
        private void IsCurrentLocationChanged(ref string locationChanged) => this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("IsCurrentLocationChanged", out locationChanged);

        private void setlocationInCookie(Guid locationid, string locationame)
        {
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = false,
                Secure = true,
            };
            this._contextAccessor.HttpContext.Response.Cookies.Append("LocationId", $"{locationid}", cookieOptions);
            this._contextAccessor.HttpContext.Response.Cookies.Append("LocationName", locationame, cookieOptions);
        }

        private void setSetUserLocationInCookie(decimal logitude, decimal latitude)
        {
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = false,
                Secure = true,
            };
            this._contextAccessor.HttpContext.Response.Cookies.Append("Logitude", $"{logitude}", cookieOptions);
            this._contextAccessor.HttpContext.Response.Cookies.Append("Latitude", $"{latitude}", cookieOptions);
        }

        private void getUserLocationfromCookie(ref string logitude, ref string latitude)
        {
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("Logitude", out logitude);
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("Latitude", out latitude);
        }

    }
}

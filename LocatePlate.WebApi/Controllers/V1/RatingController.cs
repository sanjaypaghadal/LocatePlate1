using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class RatingController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RatingController(IRestaurantService restaurantService)
        {
            this._restaurantService = restaurantService;
        }

        [HttpPost("Insert")]
        public IActionResult Insert(Ratings rating)
        {
            if (rating.UserId != Guid.Empty)
            {
                var result = this._restaurantService.GiveRating(rating);
                return Ok(result);
            }
            return Ok();
        }
    }
}

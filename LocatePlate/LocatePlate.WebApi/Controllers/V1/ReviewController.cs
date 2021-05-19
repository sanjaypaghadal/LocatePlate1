using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using LocatePlate.Service.Services.Review;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            this._reviewService = reviewService;
        }

        [HttpPost("Insert")]
        public IActionResult Insert(Reviews review)
        {
            if (review.UserId != Guid.Empty)
            {
                var existingReview = this._reviewService.GetAllByRestaurantId(review.RestaurantId);
                if (existingReview?.Any() != null && existingReview?.FirstOrDefault()?.Reviews != null)
                {
                    existingReview.FirstOrDefault().Reviews.Review = review.Review;
                    review = this._reviewService.Update(existingReview.FirstOrDefault().Reviews);
                }
                else
                {
                   
                    review = this._reviewService.Insert(review);
                }
              
            }
            return Ok(review);

        }

        [HttpGet("GetAll/{restaurantId}")]
        public IActionResult GetAll(int restaurantId)
        {
            List<RatingGroups> results=new List<RatingGroups>();
            try
            {
                results = this._reviewService.GetAllByRestaurantId(restaurantId);
            }
            catch (Exception ex)
            {

            }

            return Ok(results);
        }
    }
}

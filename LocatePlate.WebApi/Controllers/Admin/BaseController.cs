using LocatePlate.Infrastructure.Constant;
using LocatePlate.WebApi.ActionFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace LocatePlate.WebApi.Controllers.Admin
{
    //[Authorize(Policy = UserRoles.ResaurantOwner, AuthenticationSchemes = UserRoles.ResaurantOwner)]
    [Authorize(UserRoles.ResaurantOwner)]
    [RestaurantRedirectActionFilter]
    public class BaseController : Controller
    {
        protected readonly Guid UserId;
        protected readonly int RestaurantId;
        protected readonly string RestaurantName;
        protected readonly Guid LocationId;
        protected readonly string LocationName;

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (UserId == Guid.Empty)
                UserId = new Guid(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString(SessionContants.RestaurantId)))
                RestaurantId = Convert.ToInt32(contextAccessor.HttpContext.Session.GetString(SessionContants.RestaurantId));
            if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString(SessionContants.RestaurantName)))
                RestaurantName = contextAccessor.HttpContext.Session.GetString(SessionContants.RestaurantName);
            if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString(SessionContants.LocationId)))
                LocationId = new Guid(contextAccessor.HttpContext.Session.GetString(SessionContants.LocationId));
            if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString(SessionContants.LocationName)))
                LocationName = contextAccessor.HttpContext.Session.GetString(SessionContants.LocationName);
        }
    }
}

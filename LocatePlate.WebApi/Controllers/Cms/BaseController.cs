using LocatePlate.Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace LocatePlate.WebApi.Controllers.Cms
{
    //[Authorize(Policy = UserRoles.LocationAdmin, AuthenticationSchemes = UserRoles.LocationAdmin)]
    [Authorize(UserRoles.LocationAdmin)]

    public class BaseController : Controller
    {
        protected readonly Guid UserId;

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (UserId == Guid.Empty)
                UserId = new Guid(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}

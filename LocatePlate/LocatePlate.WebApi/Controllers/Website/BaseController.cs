using LocatePlate.Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace LocatePlate.WebApi.Controllers.Website
{

    //[Authorize(UserRoles.WebsiteUser)]
    public class BaseController : Controller
    {
        protected readonly Guid UserId;
        //protected TimeZoneInfo userTimeZone;
        //protected DateTime userDateTime;

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (UserId == Guid.Empty)
                    UserId = new Guid(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception ex)
            {
            }
            //try

            //{
            //    var cookieValueFromContext = contextAccessor.HttpContext.Request.Cookies["timezone"];
            //    SetUserTimeZone(cookieValueFromContext);
            //    var dateTimeUnspec = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            //    userDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, userTimeZone);
            //}
            //catch (Exception)
            //{
            //}
        }

        //private void SetUserTimeZone(string cookieValueFromContext)
        //{

        //    string jsNumberOfMinutesOffset = cookieValueFromContext;   // sending the above offset
        //    var timeZones = TimeZoneInfo.GetSystemTimeZones();
        //    var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset) * (-1);
        //    var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
        //     userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
        //}
    }
}

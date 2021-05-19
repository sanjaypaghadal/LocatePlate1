using LocatePlate.Infrastructure.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        protected readonly TimeZoneInfo clientInfoZ;
        protected readonly DateTime clientTime;

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
            clientInfoZ = clientInfoZone();
            var dateTimeUnspec = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
             clientTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, clientInfoZ);
        }

        private TimeZoneInfo clientInfoZone()
        {
            string clientInfo = this._contextAccessor.HttpContext.Request.Headers["ClientZoneInfo"];
            var clientInfoZone = SetUserTimeZone(clientInfo);
            return clientInfoZone;
        }

        private static TimeZoneInfo SetUserTimeZone(string cookieValueFromContext)
        {
            string jsNumberOfMinutesOffset = cookieValueFromContext;   // sending the above offset
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset);// * (-1);
            var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
            var userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
            return userTimeZone;
        }
    }
}

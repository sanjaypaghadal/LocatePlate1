//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LocatePlate.WebApi.Middleware
//{
//    public class CookieManager
//    {
//        private readonly RequestDelegate _next;
//        private readonly SessionOptions _options;
//        private HttpContext _context;
//        public CookieManager(RequestDelegate next, IOptions<SessionOptions> options)
//        {
//            _next = next;
//            _options = options.Value;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            _context = context;
//            var cookieValueFromContext = context.Request.Cookies.Keys.FirstOrDefault(c => c == "timezone");


//            SetUserTimeZone(cookieValueFromContext);
//            var dateTimeUnspec = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
//           var userDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, userTimeZone);
//          ///  context.Response.OnStarting(OnStartingCallBack);
//            await _next.Invoke(context);
//        }

//        //private Task OnStartingCallBack()
//        //{
//        //    var cookieOptions = new CookieOptions()
//        //    {
//        //        Path = "/",
//        //        Expires = DateTimeOffset.UtcNow.AddHours(1),
//        //        IsEssential = true,
//        //        HttpOnly = false,
//        //        Secure = false,
//        //    };
//        //    _context.Response.Cookies.Append("MyCookie", "TheValue", cookieOptions);
//        //    return Task.FromResult(0);
//        //}

//        private TimeZoneInfo SetUserTimeZone(string cookieValueFromContext)
//        {
//            string jsNumberOfMinutesOffset = cookieValueFromContext;   // sending the above offset
//            var timeZones = TimeZoneInfo.GetSystemTimeZones();
//            var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset) * (-1);
//            var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
//            var userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
//            return userTimeZone;
//        }
//    }
//}

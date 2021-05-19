using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Timings;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Controllers.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.Mvc
{
    //[Authorize]
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class TimingController : BaseController
    {
        private readonly ITimingRepository _timingRepository;
        private readonly IClientSide _clientSide;

        public TimingController(ITimingRepository timingRepository, IClientSide clientSide, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._timingRepository = timingRepository;
            this._clientSide = clientSide;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var model = this._timingRepository.GetAllByUserAndRestaurant(UserId, RestaurantId);
            foreach (var item in model)
            {
                item.IsOpen = true;
                item.StartTime = TimeZoneInfo.ConvertTimeFromUtc(item.StartTime, this._clientSide.ClientInfo);
                item.CloseTime = TimeZoneInfo.ConvertTimeFromUtc(item.CloseTime, this._clientSide.ClientInfo);
            }

            //if (model?.Count() == null || model?.Count() == 0) model = PrepareAllDayTime();
            //return View(model);

            var defaultmodel = PrepareAllDayTime();

            IEnumerable<Timing> newmodel = (from dd in defaultmodel
                                            join m in model on dd.Day equals m.Day into g
                                            from result in g.DefaultIfEmpty()
                                            select (result == null ? dd : result));

            return View(newmodel);
        }
        //post
        [HttpPost("Create")]
        public IActionResult Create(List<Timing> timing)
        {
            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                foreach (var time in timing)
                {
                    if (time.IsOpen)
                    {
                        time.UserId = UserId;
                        time.StartTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(time.StartTime, DateTimeKind.Unspecified), this._clientSide.ClientInfo);
                        time.CloseTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(time.CloseTime, DateTimeKind.Unspecified), this._clientSide.ClientInfo);
                        time.ModifiedDate = DateTime.UtcNow;
                        time.ModifiedBy = User.Identity.Name;
                        time.RestaurantId = RestaurantId;
                        if (time.Id == 0)
                        {
                            time.CreatedDate = time.ModifiedDate;
                            time.CreatedBy = time.ModifiedBy;
                            this._timingRepository.Insert(time);
                        }
                        else
                            this._timingRepository.Update(time);
                    }
                    else if (time.Id > 0)
                        this._timingRepository.Delete(time);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(timing);
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

        private TimeZoneInfo SetUserTimeZone(string cookieValueFromContext)
        {
            string jsNumberOfMinutesOffset = cookieValueFromContext;   // sending the above offset
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset) * (-1);
            var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
            var userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
            return userTimeZone;
        }

    }
}

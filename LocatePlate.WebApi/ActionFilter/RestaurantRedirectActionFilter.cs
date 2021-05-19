using LocatePlate.Infrastructure.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace LocatePlate.WebApi.ActionFilter
{
    public class RestaurantRedirectActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            object controller;
            object action;
            context.HttpContext.Request.RouteValues.TryGetValue("Controller", out controller);
            context.HttpContext.Request.RouteValues.TryGetValue("Action", out action);
            if (controller.ToString() != "Restaurant" && !context.HttpContext.Request.Path.Value.ToString().Contains("/Dashboard/"))
            {
                var RestaurantId = Convert.ToInt32(context.HttpContext.Session.GetString(SessionContants.RestaurantId));
                if (RestaurantId == 0)
                {
                    RouteValueDictionary route = new RouteValueDictionary(new
                    {
                        Controller = "Restaurant",
                        Action = "Index"
                    });

                    context.Result = new RedirectToRouteResult(route);
                }
            }
        }
    }
}

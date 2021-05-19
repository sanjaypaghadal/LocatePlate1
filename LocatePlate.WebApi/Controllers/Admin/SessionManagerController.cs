using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocatePlate.WebApi.Controllers.Admin
{
    [Route("Session")]
    public class SessionManagerController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SessionManagerController(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        [HttpPost("SetSession/{key}/{key1}/{restaurantName}")]
        public IActionResult SetSession(string key, string key1, string restaurantName, object value)
        {
            try
            {
                this._contextAccessor.HttpContext.Session.SetObjectAsJson(key, value);
                this._contextAccessor.HttpContext.Session.SetObjectAsJson(key1, restaurantName);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        [HttpPost("SetSessionString/{key}/{value}/{key1}/{value1}/{key2}/{value2}/{key3}/{value3}")]
        public IActionResult SetSessionString(string key, string value, string key1, string value1, string key2, string value2, string key3, string value3)
        {
            try
            {
                this._contextAccessor.HttpContext.Session.SetString(key, value.Trim());
                this._contextAccessor.HttpContext.Session.SetString(key1, value1.Trim());
                this._contextAccessor.HttpContext.Session.SetString(key2, value2.Trim());
                this._contextAccessor.HttpContext.Session.SetString(key3, value3.Trim());

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        [HttpGet("GetSession/{key}")]
        public IActionResult GetSession(string key)
        {
            return Ok(this._contextAccessor.HttpContext.Session.Get(key));
        }
    }
}

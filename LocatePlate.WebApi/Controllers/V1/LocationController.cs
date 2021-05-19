using LocatePlate.Service.Pages;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class LocationController : ControllerBase
    {
        private readonly IPageService _pageService;

        public LocationController(IPageService pageService)
        {
            this._pageService = pageService;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetCityByCountryProvince/{countryId}/{stateId}")]
        public IActionResult GetCityByCountryProvince(int countryId, int stateId)
        {
            try
            {
                var result = this._pageService.GetByCountryAndProvince(countryId, stateId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
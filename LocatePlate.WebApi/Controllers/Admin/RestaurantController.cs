using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Infrastructure.Geography;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Pages;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Admin
{
    //[Authorize]
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IHostingEnvironment _environment;
        private readonly IPageService _pageService;
        private readonly IHttpContextAccessor _contextAccessor;
        public RestaurantController(IRestaurantService restaurantService,
            IHostingEnvironment environment,
            IPageService pageService,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._restaurantService = restaurantService;
            this._environment = environment;
            this._pageService = pageService;
            this._contextAccessor = contextAccessor;
        }

        //get
        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public IActionResult Index(int pageindex = 0, int pagesize = 10)
        {
            var restaurants = this._restaurantService.GetAllByUser(UserId, pageindex, pagesize);
            return View(restaurants);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(int Id)
        {
            ViewBag.BasePath = Path.Combine(_environment.WebRootPath, "UploadImages");
            ViewData["Title"] = "Create";
            await setLocationDropDown();
            if (Id > 0)
            {
                ViewData["Title"] = "Edit";
                var restaurant = await this._restaurantService.GetAsync(Id);
                if (RestaurantId != restaurant.Id || RestaurantName != restaurant.Name)
                {
                    this._contextAccessor.HttpContext.Session.SetString(Infrastructure.Constant.SessionContants.RestaurantId, $"{restaurant.Id}".Trim());
                    this._contextAccessor.HttpContext.Session.SetString(Infrastructure.Constant.SessionContants.RestaurantName, restaurant.Name.Trim());
                    this._contextAccessor.HttpContext.Session.SetString(Infrastructure.Constant.SessionContants.LocationId, $"{restaurant.LocationId}");
                    this._contextAccessor.HttpContext.Session.SetString(Infrastructure.Constant.SessionContants.LocationName, $"{restaurant.CityName}");
                }
                RemoveImagesBlank(restaurant);
                return View(restaurant);
            }
            return View(new Restaurant());
        }

        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RestaurantExtend restaurant)
        {
            ModelState.Remove("Id"); // This will remove the key
            if (ModelState.IsValid)
            {
                restaurant.UserId = UserId;
                restaurant.ModifiedDate = DateTime.UtcNow;
                restaurant.ModifiedBy = User.Identity.Name;
                restaurant.Url = restaurant.RestaurantUrl;
                restaurant.Name = restaurant.Name.Trim();
                restaurant.CityName = _pageService.Get(restaurant.LocationId)?.Name;
                restaurant.ProvinceName = ((StateProvince)restaurant.Province).GetAttribute<DisplayAttribute>().Name;
                restaurant.CountryName = ((Country)restaurant.Country).GetAttribute<DisplayAttribute>().Name;
                restaurant.ResturantTypeName = (restaurant.ResturantType == ResturantType.NotSelected) ? string.Empty : restaurant.ResturantType.GetAttribute<DisplayAttribute>().Name;
                ImageProcess(restaurant);
                Restaurant restaurantModel = restaurant;
                if (restaurantModel.Id == 0)
                {
                    restaurantModel.CreatedDate = restaurantModel.ModifiedDate;
                    restaurantModel.CreatedBy = restaurantModel.ModifiedBy;
                    this._restaurantService.Insert(restaurantModel);
                }
                else
                    this._restaurantService.Update(restaurantModel);
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id > 0)
            {
                var restaurant = await this._restaurantService.GetAsync(Id);
                this._restaurantService.Delete(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private void ImageProcess(RestaurantExtend restaurant)
        {
            // if (restaurant.Name != RestaurantName) RenameFolder(restaurant.Name);
            RemoveImagesBlank(restaurant);
            if (!string.IsNullOrEmpty(restaurant.CoverImagesToDelete))
                restaurant.CoverImages = RemoveImages(restaurant.CoverImagesToDelete, restaurant.CoverImages);
            FileUpload(restaurant);
        }

        //private void RenameFolder(string restaurantName)
        //{
        //    Directory.Move(BasePath(), BasePath(restaurantName));
        //}

        private void FileUpload(Restaurant restaurant)
        {
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uploads = BasePathWithDirectoryCreation();
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        if (file.Name == "CoverImages[]")
                            restaurant.CoverImages += file.FileName + ",";
                    }
                }
            }
            if (files.Count > 0)
            {
                restaurant.CoverImages = restaurant?.CoverImages?.TrimEnd(',');
            }
        }

        private string BasePathWithDirectoryCreation(int? restaurantId = null)
        {
            var uploads = BasePath(restaurantId);
            DirectoryInfo directory = Directory.CreateDirectory(uploads);
            return uploads;
        }

        private string BasePath(int? restaurantId = null)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "UploadImages");
            uploads = $"{uploads}/{UserId}/{restaurantId ?? base.RestaurantId}";
            return uploads;
        }

        private string RemoveImages(string ImagesToDelete, string ImagesRemoveFrom)
        {
            if (!string.IsNullOrEmpty(ImagesToDelete) && !string.IsNullOrEmpty(ImagesRemoveFrom) && ImagesRemoveFrom.Length > 1 && ImagesRemoveFrom.Substring((ImagesRemoveFrom.Length - 1), 1) != ",") ImagesRemoveFrom += ",";
            foreach (var item in ImagesToDelete.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray())
            {
                var uploads = BasePath();
                var filePath = Path.Combine(uploads, item);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                ImagesRemoveFrom = ImagesRemoveFrom.Replace($"{item},", "");
            }
            return ImagesRemoveFrom;
        }

        private void RemoveImagesBlank(Restaurant restaurant)
        {
            restaurant.CoverImages = RemoveBlank(restaurant.CoverImages);
        }

        private string RemoveBlank(string arrayOne)
        {
            if (!string.IsNullOrEmpty(arrayOne))
                arrayOne = string.Join(",", arrayOne.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            return arrayOne;
        }

        private async Task setLocationDropDown()
        {
            var restaurantDropDown = new List<SelectListItem>();
            var locations = await _pageService.GetAllAsync();
            restaurantDropDown.Add(new SelectListItem { Text = "Select", Value = $"{Guid.Empty}" });
            if (locations.Any()) locations.ToList().ForEach(c => restaurantDropDown.Add(new SelectListItem { Text = c.Name, Value = $"{c.Id}" }));
            ViewBag.Locations = restaurantDropDown;
        }

        public void Dispose()
        {

        }
    }
}

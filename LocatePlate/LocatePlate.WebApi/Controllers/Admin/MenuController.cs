using LocatePlate.Infrastructure.Domain;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.MenuCategories;
using LocatePlate.Service.Menus;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Controllers.Admin;
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

namespace LocatePlate.WebApi.Controllers.Mvc
{
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IHostingEnvironment _environment;
        private readonly IMenuCategoryService _menuCategoryService;

        public MenuController(IMenuService menuService, IHostingEnvironment environment, IMenuCategoryService menuCategoryService,
        IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._menuService = menuService;
            this._environment = environment;
            this._menuCategoryService = menuCategoryService;
        }

        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public IActionResult Index(int pageindex = 0, int pagesize = 10)
        {
            ViewBag.IsCategoryAvailable = this._menuCategoryService.GetAllByUserAndRestaurant(UserId, RestaurantId).Any();
            //var menus = this._menuService.GetAllByUserAndRestaurant(UserId, RestaurantId, pageindex, pagesize);
            var menus = _menuService.GetMenuList(UserId, RestaurantId, pageindex, pagesize);

            return View(menus);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(int Id)
        {
            ViewData["Title"] = "Create";
            var cat = new List<SelectListItem>();
            this._menuCategoryService.GetAllByUserAndRestaurant(UserId, RestaurantId).ToList().ForEach(c => cat.Add(new SelectListItem { Text = c.Name, Value = $"{c.Id}" }));
            ViewBag.MenuCategories = cat;
            if (Id > 0)
            {
                ViewData["Title"] = "Edit";
                var menu = await this._menuService.GetAsync(Id);
                return View(menu);
            }
            return View(new Menu());
        }

        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuExtend menu)
        {
            ModelState.Remove("Id"); // This will remove the key 

            if (ModelState.IsValid)
            {
                menu.UserId = UserId;
                menu.ModifiedDate = DateTime.UtcNow;
                menu.ModifiedBy = User.Identity.Name;
                menu.RestaurantId = RestaurantId;
                menu.RestaurantName = RestaurantName;
                menu.MenuCategoryName = _menuCategoryService?.Get(menu.MenuCategoryId)?.Name;
                menu.FoodNatureName = menu.FoodNature.GetAttribute<DisplayAttribute>().Name;
                //FileUpload(menu);
                ImageProcess(menu);
                if (menu.Id == 0)
                {
                    menu.CreatedDate = menu.ModifiedDate;
                    menu.CreatedBy = menu.ModifiedBy;
                    this._menuService.Insert(menu);
                }
                else
                {
                    //if (string.IsNullOrEmpty(menu.Images))
                    //{
                    //    var existingmenu = this._menuService.Get(menu.Id);
                    //    menu.Images = existingmenu.Images;
                    //}
                    this._menuService.Update(menu);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        private void ImageProcess(MenuExtend menu)
        {
            RemoveImagesBlank(menu);
            if (!string.IsNullOrEmpty(menu.ImagesToDelete))
                menu.Images = RemoveImages(menu.ImagesToDelete, menu.Images);
            FileUpload(menu);
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

        private void RemoveImagesBlank(Menu menu)
        {
            menu.Images = RemoveBlank(menu.Images);
        }

        private string RemoveBlank(string arrayOne)
        {
            if (!string.IsNullOrEmpty(arrayOne))
                arrayOne = string.Join(",", arrayOne.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            return arrayOne;
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id > 0)
            {
                var menu = await this._menuService.GetAsync(Id);
                this._menuService.Delete(menu);
                //var responseDeleteMenu = this._menuService.deleteMenu(Id);
                //if ((await responseDeleteMenu) == 1)
                //{
                //    ModelState.AddModelError(string.Empty, "Menu is Reserverd in order");
                //}
                //else
                //{
                DeleteFile(menu.Images);
                //}
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private void FileUpload(Menu menu)
        {
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uploads = BasePath();
                    DirectoryInfo directory = Directory.CreateDirectory(uploads);
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        menu.Images = file.FileName;
                    }
                }
            }
            if (files.Count > 0)
            {
                menu.Images = menu?.Images;
            }
        }

        private void DeleteFile(string fileName)
        {
            var upload = BasePath();
            var filePath = Path.Combine(upload, fileName);
            if (Directory.Exists(upload))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private string BasePath()
        {
            var uploads = Path.Combine(_environment.WebRootPath, "UploadImages");
            uploads = $"{uploads}/{UserId}/{RestaurantId}/Menu";
            return uploads;
        }
    }
}

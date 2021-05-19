using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.MenuCategories;
using LocatePlate.Service.Menus;
using LocatePlate.WebApi.Contracts.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Admin
{
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class CategoryController : BaseController
    {
        private readonly IMenuCategoryService _menuCategoryService;
        public CategoryController(IMenuCategoryService menuCategoryService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._menuCategoryService = menuCategoryService;
        }
        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public IActionResult Index(int pageindex = 0, int pagesize = 10)
        {
            var restaurants = this._menuCategoryService.GetAllByUserAndRestaurant(UserId, RestaurantId, pageindex, pagesize);
            return View(restaurants);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(int Id)
        {
            ViewData["Title"] = "Create";
            if (Id > 0)
            {
                ViewData["Title"] = "Edit";
                var menu = await this._menuCategoryService.GetAsync(Id);
                return View(menu);
            }
            return View(new MenuCategory());
        }
        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuCategory menuCategory)
        {
            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                menuCategory.UserId = UserId;
                menuCategory.ModifiedDate = DateTime.UtcNow;
                menuCategory.ModifiedBy = User.Identity.Name;
                menuCategory.RestaurantId = RestaurantId;
                if (menuCategory.Id == 0)
                {
                    menuCategory.CreatedDate = menuCategory.ModifiedDate;
                    menuCategory.CreatedBy = menuCategory.ModifiedBy;
                    this._menuCategoryService.Insert(menuCategory);
                }
                else
                    this._menuCategoryService.Update(menuCategory);

                return RedirectToAction(nameof(Index));
            }
            return View(menuCategory);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromServices] IMenuService menuService, int Id)
        {
            if (Id > 0)
            {
                var menu = menuService.GetByCategoryId(Id);
                
                menuService.Delete(menu.ToArray());
                var menuCategory = await this._menuCategoryService.GetAsync(Id);
                this._menuCategoryService.Delete(menuCategory);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

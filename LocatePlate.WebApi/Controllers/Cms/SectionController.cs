using LocatePlate.Model.Cms;
using LocatePlate.Service.Sections;
using LocatePlate.WebApi.Contracts.CmsRoutes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Cms
{
    [Route(CmsRoute.BaseRoute.Cms)]
    public class SectionController : BaseController
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._sectionService = sectionService;
        }

        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public async Task<IActionResult> Index(int pageindex = 0, int pagesize = 10)
        {
            var pages = await this._sectionService.GetAllAsync(pageindex, pagesize);
            return View(pages);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(Guid Id)
        {
            ViewData["Title"] = "Create";
            if (Id != Guid.Empty)
            {
                ViewData["Title"] = "Edit";
                var menu = await this._sectionService.GetAsync(Id);
                return View(menu);
            }
            return View();
        }
        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Section section)
        {
            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                section.UserId = UserId;
                section.ModifiedDate = DateTime.UtcNow;
                section.ModifiedBy = User.Identity.Name;
                if (section.Id == Guid.Empty)
                {
                    section.CreatedDate = section.ModifiedDate;
                    section.CreatedBy = section.ModifiedBy;
                    this._sectionService.Save(section);
                }
                else
                    this._sectionService.Update(section);

                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var restaurant = await this._sectionService.GetAsync(Id);
                this._sectionService.Delete(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

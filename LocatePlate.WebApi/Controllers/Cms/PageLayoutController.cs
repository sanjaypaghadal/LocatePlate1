using LocatePlate.Model.Cms;
using LocatePlate.Service.PagesLayouts;
using LocatePlate.Service.Sections;
using LocatePlate.WebApi.Contracts.CmsRoutes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Cms
{
    [Route(CmsRoute.BaseRoute.Cms)]
    public class PageLayoutController : BaseController
    {
        private readonly IPageLayoutService _pageLayoutService;
        private readonly ISectionService _sectionService;
        public PageLayoutController(IPageLayoutService pageLayoutService, ISectionService sectionService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._pageLayoutService = pageLayoutService;
            this._sectionService = sectionService;
        }

        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public async Task<IActionResult> Index(int pageindex = 0, int pagesize = 10)
        {
            var pages = await this._pageLayoutService.GetAllAsync(pageindex, pagesize);
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
                var pageLayout = await this._pageLayoutService.GetAsync(Id);
                return View(pageLayout);
            }
            return View(new PageLayout());
        }
        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PageLayout pageLayout)
        {
            ViewData["Title"] = "Create";
            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);
            var ids = new List<Guid>();
            dict["Section"].ToList().ForEach(c => ids.Add(new Guid(c.Split("MyUniqueBreakpoint")[1])));
            var allSections = GetAllSection();
            pageLayout.Section = new List<Section>();
            ids.ForEach(c => pageLayout.Section.Add(allSections.FirstOrDefault(d => d.Id == c)));

            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                pageLayout.UserId = UserId;
                pageLayout.ModifiedDate = DateTime.UtcNow;
                pageLayout.ModifiedBy = User.Identity.Name;
                if (pageLayout.Id == Guid.Empty)
                {
                    pageLayout.CreatedDate = pageLayout.ModifiedDate;
                    pageLayout.CreatedBy = pageLayout.ModifiedBy;
                    this._pageLayoutService.Save(pageLayout);
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    this._pageLayoutService.Update(pageLayout);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(pageLayout);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var pageLayout = await this._pageLayoutService.GetAsync(Id);
                this._pageLayoutService.Delete(pageLayout);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet("SectionPartial/{id}")]
        public IActionResult SectionPartial(Guid id)
        {
            var sections = new List<Section>();
            if (id != Guid.Empty)
                sections = this._pageLayoutService.GetAsync(id).GetAwaiter().GetResult().Section;
            ViewBag.SectionDropDown = GetSectionsSelectedListItem();

            return PartialView("_Section", sections);
        }

        private List<SelectListItem> GetSectionsSelectedListItem()
        {
            var sectionDropDown = new List<SelectListItem>();
            sectionDropDown.Add(new SelectListItem { Text = "Select", Value = "0" });
            GetAllSection().ToList().ForEach(c => sectionDropDown.Add(new SelectListItem { Text = c.Name, Value = $"{c.Markup}MyUniqueBreakpoint{c.Id}" }));
            return sectionDropDown;
        }

        private ICollection<Section> GetAllSection()
        {
            return this._sectionService.GetAll();
        }
    }
}

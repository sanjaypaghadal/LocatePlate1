using LocatePlate.Model.Cms;
using LocatePlate.Model.Cms.Modules;
using LocatePlate.Model.Cms.Modules.Abstract;
using LocatePlate.Model.Cms.Modules.Advertisement;
using LocatePlate.Model.Cms.Modules.Deals;
using LocatePlate.Model.Cms.Modules.Restaurants;
using LocatePlate.Model.Cms.Modules.SearchHeader;
using LocatePlate.Model.Cms.Modules.WALF;
using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Cms
{
    public partial class PageController
    {
        [HttpGet("LayoutPartial/{id}/{layoutId}")]
        public async Task<IActionResult> LayoutPartial(Guid id, Guid layoutId)
        {
            var layout = new PageLayout();
            if (layoutId != Guid.Empty)
                layout = await this._pageLayoutService.GetAsync(layoutId);

            if (id != Guid.Empty && layout.Id == Guid.Empty)
                layout = this._pageService.GetAsync(id).GetAwaiter().GetResult().PageLayout;
            layout.LocationId = id;
            ViewBag.ModuleDropDown = GetModulesSelectedListItem(id);

            return PartialView("_Layout", layout);
        }

        [HttpGet("ModulesPartial/{layoutId}/{sectionId}/{pageId}/{count}")]
        public IActionResult ModulesPartial(Guid layoutId, Guid sectionId, Guid pageId, int count)
        {
            var module = new Module();
            // get module come from the dropdown
            if (layoutId != Guid.Empty)
            {
                module = this._moduleService.GetAllAsync().GetAwaiter().GetResult().FirstOrDefault();
                // associate sectionId to it

                switch (layoutId)
                {
                    case var g when (g == DealModule._id):
                        ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.DealModule.";
                        if (module.DealModule == null) module.DealModule = new DealModule { SectionId = sectionId, PageId = pageId };
                        else
                        {
                            module.DealModule.SectionId = sectionId;
                            module.DealModule.PageId = pageId;
                        }
                        return PartialView("_DealModule", module.DealModule);
                    case var g when (g == AdvertisementModule._id):
                        ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.AdvertisementModule.";
                        module.AdvertisementModule.SectionId = sectionId;
                        module.AdvertisementModule.PageId = pageId;
                        return PartialView("_AdvertisementModule", module.AdvertisementModule);
                    case var g when (g == CardsListingModule._id):
                        ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.CardsVerticalModule.";
                        module.CardsVerticalModule.SectionId = sectionId;
                        module.CardsVerticalModule.PageId = pageId;
                        return PartialView("_RestaurantsListingModule", module.CardsVerticalModule);
                    case var g when (g == WALFModule._id):
                        ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.WALFModule.";
                        module.WALFModule.SectionId = sectionId;
                        module.WALFModule.PageId = pageId;
                        return PartialView("_WALFModule", module.WALFModule);
                    case var g when (g == SearchButtonsModule._id):
                        ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.SearchButtonsModule.";
                        module.WALFModule.SectionId = sectionId;
                        module.WALFModule.PageId = pageId;
                        return PartialView("_SearchButtonsModule", module.SearchButtonsModule);
                }
            }
            return null;
        }

        [HttpGet("ModulesPartialByModel")]
        public IActionResult ModulesPartialByModel(PageLayout pageLayout, int count)
        {
            if (pageLayout == null) return new EmptyResult();
            foreach (var item in pageLayout.Section)
            {
                if (item.Module.DealModule != null)
                {
                    ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.DealModule.";
                    return PartialView("_DealModule", item.Module.DealModule);
                }
                else if (item.Module.AdvertisementModule != null)
                {
                    ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.AdvertisementModule.";
                    return PartialView("_AdvertisementModule", item.Module.AdvertisementModule);
                }
                else if (item.Module.CardsVerticalModule != null)
                {
                    ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.CardsVerticalModule.";
                    return PartialView("_RestaurantsListingModule", item.Module.CardsVerticalModule);
                }
                else if (item.Module.WALFModule != null)
                {
                    ViewBag.prefix = $"Page.PageLayout.Section[{count}].Module.WALFModule.";
                    return PartialView("_WALFModule", item.Module.WALFModule);
                }
            }
            return null;
        }

        private List<SelectListItem> GetModulesSelectedListItem(Guid id)
        {
            var sectionDropDown = new List<SelectListItem>();
            sectionDropDown = this._contextAccessor.HttpContext.Session.GetObjectFromJson<List<SelectListItem>>("ModuleSelectedList");
            if (sectionDropDown == null)
            {
                var modules = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(x =>
                                       typeof(IModuleEntity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                   .Select(Activator.CreateInstance).Cast<IModuleEntity>().ToList();

                sectionDropDown = new List<SelectListItem>();
                sectionDropDown.Add(new SelectListItem { Text = "Select", Value = "0" });
                modules.ToList().ForEach(c => sectionDropDown.Add(new SelectListItem { Text = c.GetType().Name, Value = $"{c.Id}", Selected = c.Id == id }));
                this._contextAccessor.HttpContext.Session.SetObjectAsJson("ModuleSelectedList", sectionDropDown);
                sectionDropDown = this._contextAccessor.HttpContext.Session.GetObjectFromJson<List<SelectListItem>>("ModuleSelectedList");
            }
            return sectionDropDown;
        }

        private List<SelectListItem> GetSectionsSelectedListItem(Guid? Id)
        {
            var sectionDropDown = new List<SelectListItem>();
            sectionDropDown.Add(new SelectListItem { Text = "Select", Value = "0" });
            var layouts = GetAllLayout().ToList();
            layouts.ForEach(c => sectionDropDown.Add(new SelectListItem { Text = c.Name, Value = $"{c.Id}", Selected = Id.GetValueOrDefault() == c.Id }));
            return sectionDropDown;
        }

        private ICollection<PageLayout> GetAllLayout()
        {
            return this._pageLayoutService.GetAll();
        }
    }
}

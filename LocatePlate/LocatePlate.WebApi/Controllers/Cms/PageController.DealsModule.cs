using LocatePlate.Model.Cms.Modules.Deals;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Cms
{
    public partial class PageController
    {
        [HttpGet("Deals/{prefix}/{count}")]
        public IActionResult Deals(string prefix, int count)
        {
            ViewBag.DealCount = count;
            ViewBag.InnerPrefix = $"{prefix}Deals[{count}].";
            return PartialView("_Deals");
        }

        [NonAction]
        public async Task SaveDeals(DealModule dealModules, string url, int section)
        {
            await SaveImages(dealModules, url, section);
            var deals = this._discountService.GetByUrl(url);
            foreach (var deal in dealModules.Deals)
            {
                var dealcurrent = deals.FirstOrDefault(a => a.PromoCode == deal.Name);
                if (dealcurrent == null)
                    await this._discountService.SaveAsync(new Model.RestaurantDomain.Discount { PromoCode = deal.Name, MinimumPrice = deal.MinimumPrice, IsCustom = false, TermAndCondition = deal.Description, Percent = deal.Percentage, DealUrl = deal.Url, Price = deal.Price, CreatedDate = DateTime.UtcNow, ValidFrom = deal.ValidFrom, ValidTo = deal.ValidTo, LocationUrl = url });
                else if (dealcurrent != null && dealcurrent.Id > 0)
                {
                    dealcurrent.PromoCode = deal.Name;
                    dealcurrent.MinimumPrice = deal.MinimumPrice;
                    dealcurrent.TermAndCondition = deal.Description;
                    dealcurrent.Percent = deal.Percentage;
                    dealcurrent.DealUrl = deal.Url;
                    dealcurrent.Price = deal.Price;
                    dealcurrent.ValidFrom = deal.ValidFrom;
                    dealcurrent.ValidTo = deal.ValidTo;
                    dealcurrent.LocationUrl = url;

                    this._discountService.Update(dealcurrent);
                }
            }

        }

        [NonAction]
        public async Task<string> GenerateSearchButtonModuleMarkup(DealModule dealModules, string url, int section)
        {
            try
            {
                await SaveDeals(dealModules, url, section);
            }
            catch (Exception ex)
            {
            }

            var markup = string.Empty;
            var deals = new StringBuilder();
            var mobileDeals = new StringBuilder();
            foreach (var deal in dealModules.Deals.OrderBy(c => c.Order))
                if (dealModules.IsVertical)
                {
                    deals.Append($@"<div class='swiper-slide'><a href='/restaurants/{url}/deals/{deal.Url}'><img src='/CMS/{url}/{deal.Images}' alt='{deal.Name}' class='border-radius-4'></a></div>");
                    mobileDeals.Append($@"<li><a href='/restaurants/{url}/deals/{deal.Url}'><img src='/CMS/{url}/{deal.Images}' alt='{deal.Name}' class='border-radius-4'></a></li>");
                }
                else
                {
                    deals.Append($@"<li class='flex-box border-radius-4'>
                                    <a href='/restaurants/{url}/deals/{deal.Url}'>
                                    <div class='d-flex'>
                                        <div class='best-deals-img'>
                                            <img src='/CMS/{url}/{deal.Images}' />
                                        </div>
                                        <div class='padding-10 best-deals-text'>
                                            <span class='font-16 bold-600 text-dark'>{deal.Name}</span>
                                            <p class='mobile-hide text-dark'>{deal.Description}</p>
                                        </div>
                                    </div>
                                    </a>
                                </li>");
                }

            if (dealModules.IsVertical)
                markup = $@"<section class='container my-5'>
                        <div class='swiper-2'>
                            <div class='pb-3'>
                                <h1 class='mobile-heading'>{dealModules.Name}</h1>
                            </div>
                            <div class='swiper-container'>
                                <div class='swiper-wrapper'>
                                    {deals}
                                </div>
                                <!-- Add Arrows -->
                                <div class='swiper-button-next'></div>
                                <div class='swiper-button-prev'></div>
                            </div>
                            <ul class='best-offer-section pl-0'>
                               {mobileDeals}
                            </ul>
                        </div>
                    </section>";
            else markup = $@"<section class='container my-5'>
                                <div class='best-deals-section'>
                                    <div class='pb-3'>
                                        <h1 class='mobile-heading'>{dealModules.Name}</h1>
                                    </div>
                                    <ul class='w-100 d-flex flex-wrap best-deal-header pl-0'>
                                      {deals}
                                    </ul>
                               </div>
                         </section>";
            return markup;
        }

        private async Task SaveImages(DealModule dealModules, string url, int section)
        {
            var files = HttpContext.Request.Form.Files;

            for (int j = 0, i = 0; i < files.Count(); i++)
            {
                var file = files[i];
                if (file.Length > 0 && file.Name.Contains($"Section[{section}].Module.DealModule.Deals"))
                {
                    var uploads = BasePath(url);
                    var filePath = Path.Combine(uploads, file.FileName);
                    dealModules.Deals[j].Images = file.FileName; j++;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
        }

        private string BasePath(string url)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "CMS");
            uploads = $"{uploads}/{url}";
            Directory.CreateDirectory(uploads);
            return uploads;
        }
    }
}

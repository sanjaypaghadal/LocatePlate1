using LocatePlate.Model.Cms.Modules.SearchHeader;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace LocatePlate.WebApi.Controllers.Cms
{
    public partial class PageController
    {
        [HttpGet("SearchButton/{prefix}/{count}")]
        public IActionResult SearchButton(string prefix, int count)
        {
            ViewBag.ButtonCount = count;
            ViewBag.InnerPrefix = $"{prefix}Buttons[{count}].";
            return PartialView("_SearchButton");
        }

        public string GenerateSearchButtonModuleMarkup(SearchButtonsModule module, string url)
        {
            // buttons
            var buttons = new StringBuilder();
            foreach (var button in module.Buttons.OrderBy(c => c.Order))
                buttons.Append($@"<div class='looking-section'>
                                   <a href = '/restaurants/{url}/keywords/{button.Keyword}' class='text-white'><h5 class='text-wrap'>{button.Name}</h5></a>
                                  </div>");

            // Get all the restaurants
            var markup = $@" <div class='d-flex justify-content-between mt-5 w-75 mx-auto location-section-header'>
                                 {buttons}
                           </div>";

            return markup;
        }
    }
}

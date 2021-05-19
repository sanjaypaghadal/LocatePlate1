using LocatePlate.WebApi.Contracts.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Mvc
{
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

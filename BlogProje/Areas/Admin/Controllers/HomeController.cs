using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/")]
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {

            ViewBag.UserName = User.Identity?.Name;
            return View();
        }
    }
}

using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        IBlogService _blogManager;
        public BlogController(IBlogService BlogManager)
        {
            _blogManager = BlogManager;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            var blog = _blogManager.GetAdmin50Blog();
            return View(blog);
        }
    }
}

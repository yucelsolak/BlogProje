using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        IBlogCategoryService _blogCategoryService;
        public CategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            var category=_blogCategoryService.GetCategoriesWithBlogCount();
            
            return View(category);
        }
    }
}

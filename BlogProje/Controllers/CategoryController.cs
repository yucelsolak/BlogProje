using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BlogCategoryManager _blogCategoryManager;

        public CategoryController(BlogCategoryManager blogCategoryManager)
        {
            _blogCategoryManager = blogCategoryManager;
        }

        public IActionResult CategoryList()
        {
            var categories = _blogCategoryManager.GetCategoriesWithBlogCount();
            return View(categories);
        }
    }
}

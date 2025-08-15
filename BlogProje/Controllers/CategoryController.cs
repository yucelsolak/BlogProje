using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _categoryService;

        public CategoryController(IBlogService blogService, IBlogCategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        [HttpGet("/Category/{slug}")]
        public IActionResult Index(string slug)
        {
            // (Opsiyonel) Kategori var mı kontrolü
            var category = _categoryService.GetBySlug(slug);

            var model = _blogService.GetByCategory(category.CategoryId); // List<BlogListDto>
            ViewBag.CategoryName = category.CategoryName; // başlıkta kullanırsın
            ViewData["Title"] = category.CategoryName+" Blogları ";
            return View(model); // Views/Category/Index.cshtml
        }
    }
}

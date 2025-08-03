using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Controllers
{
    public class BlogController : Controller
    {
        IBlogService _blogManager;
        IBlogCategoryService _blogCategoryService;
        public BlogController(IBlogService blogservice, IBlogCategoryService blogCategoryService)
        {
            _blogManager = blogservice;
            _blogCategoryService = blogCategoryService;
        }
        public IActionResult Index()
        {
            var blog = _blogManager.GetAllBlog();

            return View(blog);
        }
        [HttpGet("/BlogDetail/{Slug}")]
        public IActionResult BlogDetail(string Slug)
        {
            var blog = _blogManager.GetBlogDetail(Slug);

            _blogManager.IncrementViewCount(blog.BlogId);

            var category=_blogCategoryService.TGetByID(blog.CategoryId);
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategorySlug = category.Slug;
            return View(blog);
        }
        public IActionResult CokOkunanlar()
        {
            var blog = _blogManager.GetMostRead();
            return View(blog);
        }
        }
}

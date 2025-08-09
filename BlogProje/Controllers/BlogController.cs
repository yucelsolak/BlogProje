using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogProje.Controllers
{
    public class BlogController : Controller
    {
        IBlogService _blogManager;
        IBlogCategoryService _blogCategoryService;
        ISlugService _slugService;
        public BlogController(IBlogService blogservice, IBlogCategoryService blogCategoryService, ISlugService slugService)
        {
            _blogManager = blogservice;
            _blogCategoryService = blogCategoryService;
            _slugService = slugService;
        }
        public IActionResult Index()
        {
            var blog = _blogManager.GetAllBlog();

            return View(blog);
        }
        [HttpGet("/BlogDetail/{Slug}")]
        public async Task<IActionResult> BlogDetail(string Slug)
        {
            var blog = _blogManager.GetBlogDetail(Slug);

            _blogManager.IncrementViewCount(blog.BlogId);

            var category=_blogCategoryService.TGetByID(blog.CategoryId);
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategorySlug =await _slugService.GetSlugAsync("Category", blog.CategoryId);
            return View(blog);
        }
        public IActionResult CokOkunanlar()
        {
            var blog = _blogManager.GetMostRead();
            return View(blog);
        }
        }
}

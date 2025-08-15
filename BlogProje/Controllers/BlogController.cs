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
        IKeywordService _keywordService;
        public BlogController(IBlogService blogservice, IBlogCategoryService blogCategoryService, ISlugService slugService, IKeywordService keywordService)
        {
            _blogManager = blogservice;
            _blogCategoryService = blogCategoryService;
            _slugService = slugService;
            _keywordService = keywordService;
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
            ViewBag.Tags = _keywordService.GetLinksForBlog(blog.BlogId);
            ViewData["Title"]=blog.Title;
            return View(blog);
        }
        public IActionResult CokOkunanlar()
        {
            var blog = _blogManager.GetMostRead();
            ViewData["Title"] = "Çok Okunanlar";
            return View(blog);
        }
        [HttpGet("/tag/{slug}")]
        public IActionResult Tag(string slug)
        {
            var s = _slugService.GetBySlug("Keyword", slug);
            if (s == null) return NotFound();

            ViewBag.TagName = _keywordService.TGetByID(s.EntityId)?.KeywordName;
            var model = _blogManager.GetBlogListByKeywordId(s.EntityId);
            ViewData["Title"] = ViewBag.TagName;
            return View("Tag", model);
        }
    }
}

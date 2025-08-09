using Business.Abstract;
using Core.Extensions;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace BlogProje.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        IBlogService _blogManager;
        IBlogCategoryService _blogCategoryManager;
        public BlogController(IBlogService BlogManager, IBlogCategoryService blogCategoryManager)
        {
            _blogManager = BlogManager;
            _blogCategoryManager = blogCategoryManager;
        }
        // Sayfa ilk açılışı
        [HttpGet]
        [Area("Admin")]
        public IActionResult Index()
        {
            var blogs = _blogManager.SearchBlogAdmin(null); // GetAdmin50Blog() döner
            return View(blogs); // <-- View döndür!
        }

        // AJAX araması
        [HttpGet]
        [Area("Admin")]
        public IActionResult Search(string searchTerm)
        {
            var blogs = _blogManager.SearchBlogAdmin(searchTerm);
            return PartialView("_BlogList", blogs);
        }

        [Area("Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var blog=_blogManager.TGetByID(id);
            if (blog == null) return NotFound();

            _blogManager.TDelete(blog);
            return RedirectToAction("Index", "Blog", new { area = "Admin" });
        }


        [HttpGet]
        [Area("Admin")]
        public ActionResult AddBlog()
        {
            var categories = _blogCategoryManager.TGetList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            var model = new AddUpdateBlogDto
            {
                Image = string.Empty  // Modelin Image alanını boş olarak başlatıyoruz
            };
            return View(model);

        }
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> AddBlog(AddUpdateBlogDto model)
        {
            var fileName = await _blogManager.SaveBlogImage(model.BlogImage, model.Title);
            if (fileName != null)
            {
                model.Image = fileName;
            }

            var categories = _blogCategoryManager.TGetList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            _blogManager.AddBlog(model);
            return RedirectToAction("Index", "Blog", new { area = "Admin" });

        }
        [HttpGet]
        [Area("Admin")]
        public IActionResult Edit(int id)
        {
            var blog=_blogManager.TGetByID(id);
            if (blog == null) { return NotFound(); }
            var dto = new AddUpdateBlogDto
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Image = blog.Image,
                Description = blog.Description,
                Status= blog.Status,
                CategoryId= blog.CategoryId
            };
            var categories = _blogCategoryManager.TGetList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            ViewBag.ActionName = "Edit"; // View'da BeginForm bunu kullanıyor
            return View("AddBlog", dto); // Aynı View dosyasını kullanıyorsun
        }
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Edit(AddUpdateBlogDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActionName = "Edit";
                return View("AddBlog", dto); 
            }

            var entity = new Blog
            {
                BlogId = dto.BlogId,
                Title = dto.Title,
                Image = dto.Image,
                Description = dto.Description,
                Status = dto.Status,
                CategoryId = dto.CategoryId
            };

           await _blogManager.BlogWithSlugUpdate(entity, dto.Title,dto.BlogImage );
            return RedirectToAction("Index", "Blog", new { area = "Admin" });
        }
    }
}

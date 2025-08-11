using Business.Abstract;
using Business.Constants;
using Core.Extensions;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

           var result= _blogManager.TDelete(blog);
            TempData["BlogDeleted"] = result.Message;
            return RedirectToAction("Index", "Blog", new { area = "Admin" });
        }


        [HttpGet]
        [Area("Admin")]
        public ActionResult AddBlog()
        {
            ViewBag.Categories = BuildCategoryList();

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

            var vres = _blogManager.ValidateForAdd(model);
            if (!vres.Success)
            {
                foreach (var e in vres.Errors)
                    ModelState.AddModelError(e.Field, e.Message);

                ViewBag.Categories = BuildCategoryList(); // "0 - Kategori Seçin" dahil
                return View("AddBlog", model);
            }
            else
            {
                var result =_blogManager.AddBlog(model);
                TempData["BlogAdded"] = result.Message;
                return RedirectToAction("Index", "Blog", new { area = "Admin" });
            }
                

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
            ViewBag.Categories = BuildCategoryList();

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

            var vres = _blogManager.ValidateForAdd(dto);
            if (!vres.Success)
            {
                foreach (var e in vres.Errors)
                    ModelState.AddModelError(e.Field, e.Message);

                ViewBag.Categories = BuildCategoryList(); // "0 - Kategori Seçin" dahil
                return View("AddBlog", dto);
            }
            else
            { 
                var entity = new Blog
                {
                    BlogId = dto.BlogId,
                    Title = dto.Title,
                    Image = dto.Image,
                    Description = dto.Description,
                    Status = dto.Status,
                    CategoryId = dto.CategoryId
                };
                var result = await _blogManager.BlogWithSlugUpdate(entity, dto.Title, dto.BlogImage);
                TempData["BlogUpdated"] = result.Message;
                return RedirectToAction("Index", "Blog", new { area = "Admin" });
            }
        }
        private IEnumerable<SelectListItem> BuildCategoryList()
        {
            var categories = _blogCategoryManager.TGetList();
            var list = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            list.Insert(0, new SelectListItem { Value = "0", Text = "Kategori Seçin" });
            return list;
        }
    }
}

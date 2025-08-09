using Business.Abstract;
using Core.Extensions;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BlogProje.Areas.Admin.Controllers
{
    
    public class CategoryController : Controller
    {
        IBlogCategoryService _blogCategoryService;
        IBlogService _blogService;
        public CategoryController(IBlogCategoryService blogCategoryService, IBlogService blogService)
        {
            _blogCategoryService = blogCategoryService;
            _blogService = blogService;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            var category=_blogCategoryService.GetCategoriesWithBlogCount();
            
            return View(category);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {   var category = _blogCategoryService.TGetByID(id);

            try
            {
                // Silme işlemini manager üzerinden çağırıyoruz
                _blogCategoryService.TDelete(category);
                TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
            }
            catch (InvalidOperationException ex)
            {
                // Manager'dan dönen hatayı burada yakalıyoruz
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", "Category", new { area = "Admin" });

        }

        [Area("Admin")]
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult AddCategory(AddCategoryDto model)
        {
            _blogCategoryService.AddCategory(model);
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }

        [HttpGet]
        [Area("Admin")]
        public IActionResult Edit(int id)
        {
            var category = _blogCategoryService.TGetByID(id);
            if (category == null)
            {
                return NotFound();
            }

            var dto = new UpdateCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Status = category.Status
                // Slug alanı yoksa zaten gerek yok
            };

            ViewBag.ActionName = "Edit"; // View'da BeginForm bunu kullanıyor
            return View("AddCategory", dto); // Aynı View dosyasını kullanıyorsun
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult Edit(UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActionName = "Edit";
                return View("AddCategory", dto);
            }

            var entity = new BlogCategory
            {
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                Status = dto.Status
            };

            _blogCategoryService.CategoryWithSlugUpdate(entity, dto.CategoryName); // Slug içeride otomatik üretiliyor

            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
        
        }
}

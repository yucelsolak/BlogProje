using Business.Abstract;
using Core.Extensions;
using Core.Utilities.Results;
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
            

            var result = _blogCategoryService.TDelete(category);

            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["SuccessCategory"] = result.Message;

            return RedirectToAction("Index", "Category", new { area = "Admin" });

        }

        [Area("Admin")]
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View("AddCategory", new UpdateCategoryDto());
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult AddCategory(UpdateCategoryDto model)
        {
            var addDto = new AddCategoryDto { CategoryName = model.CategoryName };

            var result = _blogCategoryService.AddCategory(addDto);
            
            if (!result.Success)
            {
                ModelState.AddModelError("CategoryName", result.Message);
                return View("AddCategory", model);// formu aynı hatayla geri göster
            }

            TempData["Success"] = result.Message;
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

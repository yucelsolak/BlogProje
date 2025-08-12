using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Admin;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            var admin = _adminService.TGetList();
            return View(admin);
        }
        [Area("Admin")]
        public IActionResult AddAdmin()
        {
            return View("AddAdmin", new AddUpdateAdmin());
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult AddAdmin(AddUpdateAdmin dto)
        {
            try
            {
                var res = _adminService.AddAdmin(dto);
                if (!res.Success) { /* ErrorResult -> ModelState */ }
                TempData["success"] = res.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var e in ex.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View("AddAdmin", dto);
            }
        }

        [HttpGet]
        [Area("Admin")]
        public IActionResult Edit(int id)
        {
            var admin = _adminService.TGetByID(id);
            var dto = new AddUpdateAdmin()
            { 
                AdminId = admin.AdminId,
                Name = admin.Name,
                Status = admin.Status,
                Email = admin.Email,
            };
            ViewBag.ActionName = "Edit"; // View'da BeginForm bunu kullanıyor
            return View("AddAdmin", dto);
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult Edit(AddUpdateAdmin dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActionName = "Edit";
                return View("AddAdmin", dto);
            }
            try
            {
                var res = _adminService.UpdateAdmin(dto);
                if (!res.Success) { /* ErrorResult -> ModelState */ }
                TempData["AdminUpdated"] = res.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var e in ex.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View("AddAdmin", dto);
            }
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var admin=_adminService.TGetByID(id);
            if (admin == null) return NotFound();

            var result=_adminService.TDelete(admin);
                TempData["AdminDeleted"] = result.Message;
            return RedirectToAction("Index", "Admin", new { area = "Admin" });
        }
        }
}

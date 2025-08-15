using AutoMapper;
using BlogProje.Areas.Admin.Models;
using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Admin;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Entities.DTOs.Claim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BlogProje.Areas.Admin.Controllers
{
    public class AdminController : AdminBaseController
    {
        IAdminService _adminService;
        IClaimService _claimService;
        IUserClaimService _userClaimService;
        IMapper _mapper;
        public AdminController(IAdminService adminService, IClaimService claimService, IUserClaimService userClaimService,IMapper mapper)
        {
            _adminService = adminService;
            _claimService = claimService;
            _userClaimService = userClaimService;
            _mapper = mapper;
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
                if (!res.Success) { }
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
                UserId = admin.UserId,
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
            var admin = _adminService.TGetByID(id);
            if (admin == null) return NotFound();

            var result = _adminService.TDelete(admin);
            TempData["AdminDeleted"] = result.Message;
            return RedirectToAction("Index", "Admin", new { area = "Admin" });
        }

        [HttpGet]
        [Area("Admin")]
        public IActionResult AddUserClaim(int userId)
        {
            var user=_adminService.TGetByID(userId);
            ViewBag.Name=user.Name;
            ViewBag.UserId= user.UserId;
            var userClaims=_claimService.GetClaims(userId);
            ViewBag.SelectedIds = userClaims.Select(x => x.OperationClaimId).ToHashSet();

            var allClaims = _claimService.TGetList();
            return View(allClaims);
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult AddUserClaim(int userId, List<int> SelectedClaimIds)
        {// Hiç checkbox seçilmediyse null gelebilir:
            SelectedClaimIds ??= new List<int>();

            var existing = _claimService.GetClaims(userId) // List<AddUpdateUserClaim>
                               .Select(x => x.OperationClaimId)
                               .ToList();

            // Fark kümeleri
            var toAdd = SelectedClaimIds.Except(existing).ToList();  // yeni işaretlenenler
            var toRemove = existing.Except(SelectedClaimIds).ToList();  // kaldırılanlar

            // Servis çağrıları (aşağıda örnek implementasyon var)
            if (toAdd.Any())
                _claimService.AddUserClaims(userId, toAdd);

            if (toRemove.Any())
                _claimService.RemoveUserClaims(userId, toRemove);

            TempData["success"] = "Görevler güncellendi.";
            return RedirectToAction("Index", "Admin", new { area = "Admin" });
        }
    }
        
}
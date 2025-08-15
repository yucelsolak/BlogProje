using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Entities.Concrete;
using Entities.DTOs.Admin;
using Entities.DTOs.Category;
using Entities.DTOs.Claim;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    public class ClaimController : AdminBaseController
    {
        IClaimService _claimService;
        IMapper _mapper;
        public ClaimController(IClaimService claimService,IMapper mapper)
        {
           _claimService = claimService;
            _mapper = mapper;
        }
        [Area("Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            var claim=_claimService.TGetList();
            return View(claim);
        }
        [HttpGet]
        [Area("Admin")]
        public IActionResult AddClaim()
        {
            return View("AddClaim");
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult AddClaim(AddUpdateClaim dto)
        {
            try
            {
                var result = _claimService.AddClaim(dto);
                if (!result.Success) {
                    ModelState.AddModelError(nameof(AddUpdateClaim.Name), result.Message);
                    return View("AddClaim", dto);
                }
                TempData["success"] = result.Message;
                return RedirectToAction("Index", "Claim", new { area = "Admin" });
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var e in ex.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View("AddClaim", dto);
            }
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult Delete(int id) { 
            var claim=_claimService.TGetByID(id);
            var result=_claimService.TDelete(claim);
            if (!result.Success)
                TempData["Error"] = result.Message;
            else
            TempData["ClaimDeleted"] = result.Message;
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }
        [HttpGet]
        [Area("Admin")]
        public IActionResult Edit(int id) { 
            var claim= _claimService.TGetByID(id);
            var dto = _mapper.Map<AddUpdateClaim>(claim);
            ViewBag.ActionName = "Edit"; 
            return View("AddClaim", dto);
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult Edit(AddUpdateClaim dto)
        {
            try
            {
                var result = _claimService.UpdateClaim(dto);
                if (!result.Success)
                {
                    ModelState.AddModelError(nameof(AddUpdateClaim.Name), result.Message);
                    return View("AddClaim", dto);
                }
                TempData["success"] = result.Message;
                return RedirectToAction("Index", "Claim", new { area = "Admin" });
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var e in ex.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View("AddCategory", dto);
            }
        }
        }
}

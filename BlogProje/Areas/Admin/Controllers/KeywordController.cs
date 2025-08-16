using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Entities.DTOs.Claim;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlogProje.Areas.Admin.Controllers
{
    public class KeywordController : AdminBaseController
    {
        IKeywordService _keywordService;
        IMapper _mapper;
        
        public KeywordController(IKeywordService keywordService,IMapper mapper)
        {
            _keywordService = keywordService;
            _mapper = mapper;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            var keyword = _keywordService.TGetList();
            var dto=_mapper.Map<List<KeywordLinkDto>>(keyword);
            return View(dto);
        }
        [Area("Admin")]
        public IActionResult Edit(int id)
        { 
            var keyword=_keywordService.TGetByID(id);
            var dto = _mapper.Map<KeywordLinkDto>(keyword);
            return View("Edit", dto);
        }
        [HttpPost]
        [Area("Admin")]
        public IActionResult Edit(KeywordLinkDto dto)
        {
            try
            {
                var result=_keywordService.UpdateKeyword(dto);
                if (!result.Success)
                {
                    ModelState.AddModelError(nameof(KeywordLinkDto.KeywordName), result.Message);
                    return View("Edit", dto);
                }
                TempData["KeywordUpdated"] = result.Message;
                return RedirectToAction("Index", "Keyword", new { area = "Admin" });
            }
            catch (FluentValidation.ValidationException ex)
            {

                foreach (var e in ex.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View("Edit", dto);
            }
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult Delete(int id)
        {
            var keyword = _keywordService.TGetByID(id);
            var result=_keywordService.TDelete(keyword);
            if (!result.Success)
                TempData["Error"] = result.Message;
            else
                TempData["KeywordDeleted"] = result.Message;
            return RedirectToAction("Index", "Keyword", new { area = "Admin" });
        }
        [HttpGet]
        [Area("Admin")]
        public IActionResult Search(string searchTerm)
        {
            var keywords = _keywordService.SearchKeyword(searchTerm);
            return PartialView("_KeywordList", keywords);
        }
    }
}

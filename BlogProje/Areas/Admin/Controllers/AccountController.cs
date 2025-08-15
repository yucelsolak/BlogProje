using AutoMapper;
using BlogProje.Areas.Admin.Models;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogProje.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        IClaimService _claimService;
        public AccountController(IClaimService claimService,IAuthService authService)
        {
            _claimService = claimService;
            _authService = authService;
        }

        [Area("Admin")]
        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());


        [Area("Admin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) Kullanıcı doğrulama (Business/AuthService)
            var loginResult = _authService.Login(new UserForLoginDto
            {
                Email = model.Email,
                Password = model.Password
            });

            if (!loginResult.Success)
            {
                ModelState.AddModelError("", loginResult.Message ?? "E-posta veya şifre hatalı.");
                return View(model);
            }

            var user = loginResult.Data; // Email, Name, UserId

            // 2) Kullanıcının rol adlarını Business üzerinden al (DAL join ile)
            var roleNames = _claimService.GetRoleNamesByUserId(user.UserId) ?? new List<string>();

            // 3) Admin paneline sadece Admin veya Editor girebilsin
            var isAdminOrEditor = roleNames.Any(r => r == "Admin" || r == "Editor");
            if (!isAdminOrEditor)
            {
                ModelState.AddModelError("", "Admin paneline erişim yetkiniz yok.");
                return View(model);
            }

            // 4) Cookie'ye yazılacak claim'ler
            var displayName = string.IsNullOrWhiteSpace(user.Name) ? user.Email : user.Name;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, displayName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    };

            // 5) Role claim'lerini ekle (Admin/Editor/Member vs.)
            foreach (var role in roleNames.Distinct())
            {
                if (!string.IsNullOrWhiteSpace(role))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                AllowRefresh = true
            };

            // 6) Oturum aç
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            // 7) Yönlendirme
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }


        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
        [Area("Admin")]
        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}

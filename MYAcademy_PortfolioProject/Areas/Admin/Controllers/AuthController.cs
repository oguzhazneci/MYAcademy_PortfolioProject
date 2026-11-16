using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MYAcademy_PortfolioProject.Models;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Admin alanının tamamı korumalı; giriş sayfası bu kuralın tek istisnası.
    public class AuthController : Controller
    {
        private readonly AdminCredentials _credentials;

        public AuthController(IOptions<AdminCredentials> credentials)
        {
            _credentials = credentials.Value;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Zaten giriş yapmış biri giriş sayfasını açarsa doğrudan panele gönder.
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Yapılandırmada şifre tanımlı değilse (örn. canlı sunucuda unutulduysa) kimse giremesin.
            bool isConfigured = !string.IsNullOrEmpty(_credentials.UserName)
                                && !string.IsNullOrEmpty(_credentials.Password);

            bool isValid = isConfigured
                           && model.UserName == _credentials.UserName
                           && model.Password == _credentials.Password;

            if (!isValid)
            {
                // Hangisinin yanlış olduğunu söylemiyoruz: kullanıcı adını tahmin etmeyi kolaylaştırmasın.
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            // Kimlik bilgisi: oturumdaki kullanıcıyı tanımlayan "talepler" (claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    // "Beni hatırla" işaretliyse tarayıcı kapansa da oturum sürer.
                    IsPersistent = model.RememberMe
                });

            // Sadece kendi sitemizin içindeki bir adrese geri dön (açık yönlendirme açığını önler).
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
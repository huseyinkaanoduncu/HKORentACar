using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HKORentACar.Core.Concretes.DTOs;
using HKORentACar.Core.Concretes.Entities;

namespace HKORentACar.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MusteriKayitDto dto)
        {
            if (ModelState.IsValid)
            {
                var varMi = await _userManager.FindByEmailAsync(dto.Email);
                if (varMi != null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanımda.");
                    return View(dto);
                }

                var yeniKullanici = new AppUser
                {
                    Ad = dto.Ad,
                    Soyad = dto.Soyad,
                    Email = dto.Email,
                    UserName = dto.Email, 
                    PhoneNumber = dto.Telefon
                };

                
                var result = await _userManager.CreateAsync(yeniKullanici, dto.Sifre);

                if (result.Succeeded)
                {
                    
                    var customClaims = new List<Claim>
                    {
                        new Claim("TamAd", $"{yeniKullanici.Ad} {yeniKullanici.Soyad}")
                    };

                    await _signInManager.SignInWithClaimsAsync(yeniKullanici, isPersistent: false, customClaims);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(dto);
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                ModelState.AddModelError("", "E-posta ve şifre zorunludur.");
                return View();
            }

            var kullanici = await _userManager.FindByEmailAsync(email);
            if (kullanici == null)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
                return View();
            }

            
            var result = await _signInManager.CheckPasswordSignInAsync(kullanici, sifre, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                
                var customClaims = new List<Claim>
                {
                    new Claim("TamAd", $"{kullanici.Ad} {kullanici.Soyad}")
                };

                await _signInManager.SignInWithClaimsAsync(kullanici, isPersistent: true, customClaims);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View();
        }

        // GET: Account/Logout (Çıkış Yap)
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
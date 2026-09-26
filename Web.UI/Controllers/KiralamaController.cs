using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.Enums;

namespace HKORentACar.Web.UI.Controllers
{
    [Authorize]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaService _kiralamaService;
        private readonly IAracService _aracService;

        public KiralamaController(IKiralamaService kiralamaService, IAracService aracService)
        {
            _kiralamaService = kiralamaService;
            _aracService = aracService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var kullaniciKiralamalari = await _kiralamaService.GetUserKiralamalarAsync(userId);
            return View(kullaniciKiralamalari);
        }

        [HttpGet]
        public async Task<IActionResult> Kirala(int id, string? baslangic, string? bitis)
        {
            var arac = await _aracService.GetByIdAsync(id);
            if (arac == null)
            {
                return NotFound("Kiralanacak araç bulunamadı.");
            }

            ViewBag.Arac = arac;

            // HTML5 datetime-local ("yyyy-MM-ddTHH:mm") veya standart tarih formatlarını güvenle karşıla
            if (!string.IsNullOrWhiteSpace(baslangic) &&
                DateTime.TryParse(baslangic, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedBaslangic))
            {
                ViewBag.BaslangicTarihi = parsedBaslangic;
            }
            else if (!string.IsNullOrWhiteSpace(baslangic) && DateTime.TryParse(baslangic, out var altBaslangic))
            {
                ViewBag.BaslangicTarihi = altBaslangic;
            }
            else
            {
                ViewBag.BaslangicTarihi = DateTime.Now;
            }

            if (!string.IsNullOrWhiteSpace(bitis) &&
                DateTime.TryParse(bitis, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedBitis))
            {
                ViewBag.BitisTarihi = parsedBitis;
            }
            else if (!string.IsNullOrWhiteSpace(bitis) && DateTime.TryParse(bitis, out var altBitis))
            {
                ViewBag.BitisTarihi = altBitis;
            }
            else
            {
                ViewBag.BitisTarihi = ((DateTime)ViewBag.BaslangicTarihi).AddDays(2);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kirala(int aracId, DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _kiralamaService.KiralaAsync(aracId, userId, baslangicTarihi, bitisTarihi);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                ViewBag.Arac = await _aracService.GetByIdAsync(aracId);
                ViewBag.BaslangicTarihi = baslangicTarihi;
                ViewBag.BitisTarihi = bitisTarihi;
                return View();
            }

            TempData["Basari"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IptalEt(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            bool isAdmin = User.IsInRole("Admin");

            var result = await _kiralamaService.IptalEtAsync(id, userId, isAdmin);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("yetkiniz yok"))
                    return Forbid();

                TempData["Hata"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Yonetim()
        {
            var tumKiralamalar = await _kiralamaService.GetAllKiralamalarWithDetailsAsync();
            var tumAraclar = await _aracService.GetAllAsync();

            ViewBag.ToplamCiro = tumKiralamalar.Where(k => k.Durum != KiralamaDurum.IptalEdildi).Sum(k => k.ToplamUcret);
            ViewBag.AktifKiralamaSayisi = tumKiralamalar.Count(k => k.Durum == KiralamaDurum.Aktif);
            ViewBag.ToplamAracSayisi = tumAraclar.Count;
            ViewBag.MusaitAracSayisi = tumAraclar.Count(a => a.Durum == AracDurum.Musait);

            return View(tumKiralamalar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TeslimAl(int kiralamaId, int teslimEdilenKm)
        {
            var result = await _kiralamaService.TeslimAlAsync(kiralamaId, teslimEdilenKm);

            if (!result.IsSuccess)
            {
                TempData["Hata"] = result.Message;
                return RedirectToAction(nameof(Yonetim));
            }

            TempData["Basari"] = result.Message;
            return RedirectToAction(nameof(Yonetim));
        }
    }
}
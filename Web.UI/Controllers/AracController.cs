using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.DTOs;
using HKORentACar.Core.Concretes.Enums;
using HKORentACar.Core.Utils.GenericRepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HKORentACar.Web.UI.Controllers
{
    public class AracController : Controller
    {
        private readonly IAracService _aracService;
        private readonly IUnitOfWork _unitOfWork;

        public AracController(IAracService aracService, IUnitOfWork unitOfWork)
        {
            _aracService = aracService;
            _unitOfWork = unitOfWork;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index(int? subeId, string? aramaMetni, DateTime? baslangic, DateTime? bitis)
        {
            var tumAraclar = await _aracService.GetAllAsync();

            if (subeId.HasValue && subeId.Value > 0)
            {
                tumAraclar = tumAraclar.Where(a => a.SubeId == subeId.Value).ToList();
                ViewBag.SeciliSubeId = subeId.Value;
            }

            if (!string.IsNullOrWhiteSpace(aramaMetni))
            {
                tumAraclar = tumAraclar.Where(a =>
                    (!string.IsNullOrEmpty(a.Marka) && a.Marka.Contains(aramaMetni, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(a.Model) && a.Model.Contains(aramaMetni, StringComparison.OrdinalIgnoreCase))
                ).ToList();
                ViewBag.AramaMetni = aramaMetni;
            }

            var subeler = await _unitOfWork.GetRepository<HKORentACar.Core.Concretes.Entities.Sube>().GetAllAsync();
            ViewBag.SubeListesi = new SelectList(subeler, "Id", "SehirAdi", subeId);

            // Tarihleri View'a aktarıyoruz
            ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.Bitis = bitis?.ToString("yyyy-MM-ddTHH:mm");

            return View(tumAraclar);
        }
        // GET: Arac/Create (Sadece Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await DropdownlariYukleAsync();
            return View();
        }

        // POST: Arac/Create (Sadece Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AracDto dto)
        {
            if (dto.ResimDosyasi != null && dto.ResimDosyasi.Length > 0)
            {
                var klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "araclar");

                if (!Directory.Exists(klasorYolu))
                {
                    Directory.CreateDirectory(klasorYolu);
                }

                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dto.ResimDosyasi.FileName);
                var tamYol = Path.Combine(klasorYolu, dosyaAdi);

                using (var stream = new FileStream(tamYol, FileMode.Create))
                {
                    await dto.ResimDosyasi.CopyToAsync(stream);
                }

                dto.ResimUrl = "/images/araclar/" + dosyaAdi;
            }

            ModelState.Remove("ResimDosyasi");
            ModelState.Remove("ResimUrl");

            if (string.IsNullOrEmpty(dto.ResimUrl))
            {
                dto.ResimUrl = "https://via.placeholder.com/80x50?text=Resim+Yok";
            }

            if (ModelState.IsValid)
            {
                dto.Durum = AracDurum.Musait;
                await _aracService.AddAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await DropdownlariYukleAsync();
            return View(dto);
        }

        // GET: Arac/Edit/5 (Sadece Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var arac = await _aracService.GetByIdAsync(id);
            if (arac == null)
            {
                return NotFound();
            }

            await DropdownlariYukleAsync();
            return View(arac);
        }

        // POST: Arac/Edit/5 (Sadece Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, AracDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (dto.ResimDosyasi != null && dto.ResimDosyasi.Length > 0)
            {
                var klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "araclar");

                if (!Directory.Exists(klasorYolu))
                {
                    Directory.CreateDirectory(klasorYolu);
                }

                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dto.ResimDosyasi.FileName);
                var tamYol = Path.Combine(klasorYolu, dosyaAdi);

                using (var stream = new FileStream(tamYol, FileMode.Create))
                {
                    await dto.ResimDosyasi.CopyToAsync(stream);
                }

                dto.ResimUrl = "/images/araclar/" + dosyaAdi;
            }

            ModelState.Remove("ResimDosyasi");
            ModelState.Remove("ResimUrl");

            if (ModelState.IsValid)
            {
                await _aracService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            await DropdownlariYukleAsync();
            return View(dto);
        }

        // POST: Arac/Delete/5 (Sadece Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _aracService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task DropdownlariYukleAsync()
        {
            var kategoriler = await _unitOfWork.GetRepository<HKORentACar.Core.Concretes.Entities.AracKategori>().GetAllAsync();
            var subeler = await _unitOfWork.GetRepository<HKORentACar.Core.Concretes.Entities.Sube>().GetAllAsync();

            ViewBag.KategoriListesi = new SelectList(kategoriler, "Id", "Ad");
            ViewBag.SubeListesi = new SelectList(subeler, "Id", "SehirAdi");
        }
    }
}
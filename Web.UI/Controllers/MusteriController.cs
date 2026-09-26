using HKORentACar.Core.Abstracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HKORentACar.Web.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MusteriController : Controller
    {
        private readonly IMusteriService _musteriService;

        public MusteriController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var musteriler = await _musteriService.GetAllMusterilerAsync();
            return View(musteriler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _musteriService.DeleteMusteriAsync(id);

            if (!result.IsSuccess)
            {
                TempData["Hata"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
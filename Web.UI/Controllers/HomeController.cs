using System.Diagnostics;
using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.Entities;
using HKORentACar.Core.Utils.GenericRepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAracService _aracService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IAracService aracService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _aracService = aracService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var subeler = await _unitOfWork.GetRepository<Sube>().GetAllAsync();
            ViewBag.Subeler = new SelectList(subeler, "Id", "SehirAdi");

            var araclar = await _aracService.GetAllAsync();
            return View(araclar);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
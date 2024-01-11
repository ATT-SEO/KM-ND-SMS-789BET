using FE.CLIENT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FE.CLIENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "MB66 - Khuyến mãi MB66";
            ViewData["Description"] = "Chương trình khuyến mãi của MB66 dành cho thành viên MB66 - Khuyến mãi MB66 ";
            ViewData["Keywords"] = "MB66, khuyến mãi, khuyến mãi MB66, đăng ký, hội viên ...";
            return View();
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

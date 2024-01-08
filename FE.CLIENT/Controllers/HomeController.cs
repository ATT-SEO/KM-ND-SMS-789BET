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
            ViewData["Title"] = "Jun88 - Khuyến mãi 58k";
            ViewData["Description"] = "Chương trình khuyến mãi của Jun88 dành cho thành viên Jun88 - Khuyến mãi 58k ";
            ViewData["Keywords"] = "Jun88, khuyến mãi, khuyến mãi 58k, đăng ký, hội viên ...";
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

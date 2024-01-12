using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FE.JunCMD.Client.Controllers
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

    }
}

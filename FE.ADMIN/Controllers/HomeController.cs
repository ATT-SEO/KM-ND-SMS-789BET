using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FE.ADMIN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly UserDTO _userDTO;
        public HomeController(ITokenProvider TokenProvider)
        {
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Login", "Auth");
            }else
            {
                ViewBag.LoginUser = _userDTO;
                return View();
            }
            
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

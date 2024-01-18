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
        private UserDTO _userDTO;
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
                return View(_userDTO);
            }
            
        }
    }
}

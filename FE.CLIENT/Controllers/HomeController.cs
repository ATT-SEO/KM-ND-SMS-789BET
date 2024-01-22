using FE.CLIENT.Models;
using FE.CLIENT.Services;
using FE.CLIENT.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FE.CLIENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISiteService _siteService;
        public HomeController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IActionResult> Index()
        {
            ResponseDTO _res = await _siteService.GetByProjectCode("FREE66");
            Console.WriteLine(JsonConvert.SerializeObject(_res));
            if (_res != null && _res.IsSuccess)
            {
                List<SiteDTO> _siteService = JsonConvert.DeserializeObject<List<SiteDTO>>(_res.Result.ToString());
                for (int i = 0; i < _siteService.Count; i++)
                {
                    if (_siteService[i].Status==false)
                    {
                        return Redirect("https://att-maintenance.web.app/");
                    }
                }
            }

            ViewData["Title"] = "MB66 - Khuyến mãi MB66";
            ViewData["Description"] = "Chương trình khuyến mãi của MB66 dành cho thành viên MB66 - Khuyến mãi MB66 ";
            ViewData["Keywords"] = "MB66, khuyến mãi, khuyến mãi MB66, đăng ký, hội viên ...";
            return View();
        }
        
    }
}

using FE.JunCMD.Client.Models;
using FE.JunCMD.Client.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FE.JunCMD.Client.Controllers
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
            ResponseDTO _res = await _siteService.GetByProjectCode("K58");
            Console.WriteLine(JsonConvert.SerializeObject(_res));
            if (_res != null && _res.IsSuccess)
            {
                List<SiteDTO> _siteService = JsonConvert.DeserializeObject<List<SiteDTO>>(_res.Result.ToString());
                for (int i = 0; i < _siteService.Count; i++)
                {
                    if (_siteService[i].Status == false)
                    {
                        return Redirect("https://att-maintenance.web.app/");
                    }
                }
            }

            ViewData["Title"] = "Jun88 - Khuyến mãi 58k";
            ViewData["Description"] = "Chương trình khuyến mãi của Jun88 dành cho thành viên Jun88 - Khuyến mãi 58k ";
            ViewData["Keywords"] = "Jun88, khuyến mãi, khuyến mãi 58k, đăng ký, hội viên ...";
            return View();
        }

    }
}

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
            ResponseDTO _res = await _siteService.GetByProjectCode("bo_789bet");
            Console.WriteLine(JsonConvert.SerializeObject(_res));
            if (_res != null && _res.IsSuccess)
            {
                SiteDTO _siteService = JsonConvert.DeserializeObject<SiteDTO>(_res.Result.ToString());
                if (_siteService.Status==false)
                {
                    return Redirect("https://att-maintenance.web.app/");
                }
                ViewData["Title"] = _siteService.Name;
                ViewData["Description"] = "Chương trình khuyến mãi của 789BET dành cho thành viên 789BET - Khuyến mãi 789BET ";
                ViewData["Keywords"] = "789BET, khuyến mãi, khuyến mãi 789BET, đăng ký, hội viên ...";
                return View();
            }

            ViewData["Title"] = "789BET - Khuyến mãi 789BET";
            ViewData["Description"] = "Chương trình khuyến mãi của 789BET dành cho thành viên 789BET - Khuyến mãi 789BET ";
            ViewData["Keywords"] = "789BET, khuyến mãi, khuyến mãi 789BET, đăng ký, hội viên ...";
            return View();
        }
        
    }
}

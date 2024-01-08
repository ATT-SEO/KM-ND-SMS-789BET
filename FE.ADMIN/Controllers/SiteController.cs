using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace FE.ADMIN.Controllers
{
    public class SiteController : Controller
    {
        // GET: SiteController
        private readonly ISiteService _site;
        public SiteController(ISiteService siteService)
        {
            _site = siteService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<SiteDTO>? siteList = new List<SiteDTO>();
                ResponseDTO? res = await _site.GetAllAsync();
                if (res != null && res.IsSuccess)
                {
                    siteList = JsonConvert.DeserializeObject<List<SiteDTO>>(Convert.ToString(res.Result)!);
                    return View(siteList);
                }
                else
                {
                    TempData["error"] = res?.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SiteDTO siteDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseDTO? res = await _site.CreateAsync(siteDTO);
                    if (res != null && res.IsSuccess)
                    {
                        TempData["success"] = "Create New Site Successfully";
                        return RedirectToAction("Index", "Site");
                    }
                    else
                    {
                        TempData["error"] = "Create New Site Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View("Create");
        }

        // GET: SiteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public async Task<IActionResult> Edit(int Id)
        {

            try
            {
                ResponseDTO? res = await _site.GetSiteByIDAsync(Id);
                if (res != null && res.IsSuccess)
                {
                    SiteDTO? siteDTO = JsonConvert.DeserializeObject<SiteDTO>(Convert.ToString(res.Result));
                    return View(siteDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SiteDTO siteDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseDTO? res = await _site.EditAsync(siteDTO);
                    if (res != null && res.IsSuccess)
                    {
                        TempData["success"] = "Edit Site Successfully";
                        return RedirectToAction("Index", "Site");
                    }
                    else
                    {
                        TempData["error"] = "Edit Site Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View("Edit");
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ResponseDTO? res = await _site.DeleteAsync(id);
                if (res != null && res.IsSuccess)
                {
                    SiteDTO? siteDTO = JsonConvert.DeserializeObject<SiteDTO>(Convert.ToString(res.Result));
                    TempData["success"] = "Delete Site Successfully"; ;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Index", "Site");
        }
    }
}

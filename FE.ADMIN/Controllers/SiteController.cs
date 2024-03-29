using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace FE.ADMIN.Controllers
{
    public class SiteController : Controller
    {
        // GET: SiteController
        private readonly ISiteService _site;
        private readonly ITokenProvider _tokenProvider;
        private UserDTO _userDTO;

        public SiteController(ISiteService siteService, ITokenProvider TokenProvider)
        {
            _site = siteService;
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.LoginUser = _userDTO;
                ResponseDTO _siteRes = await _site.GetByProjectCode("bo_789bet");
                if (_siteRes!=null && _siteRes.IsSuccess)
                {
                    return View(JsonConvert.DeserializeObject<List<SiteDTO>>(Convert.ToString(_siteRes.Result)));
                }
                else
                {
                    TempData["error"] = _siteRes?.Message;
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
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SiteDTO siteDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.LoginUser = _userDTO;
                    //Link to ProjectCode Automatically
                    siteDTO.Project = "bo_789bet";
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
                ViewBag.LoginUser = _userDTO;
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
                    ViewBag.LoginUser = _userDTO;
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
                ViewBag.LoginUser = _userDTO;
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

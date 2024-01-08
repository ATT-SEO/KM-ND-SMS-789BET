using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FE.ADMIN.Controllers
{
    public class PhoneNumberController : Controller
    {
        private readonly IPhoneNumberService _phoneNumber;
        private readonly ISiteService _site;

        public PhoneNumberController(IPhoneNumberService phoneNumber, ISiteService siteService)
        {
            _phoneNumber = phoneNumber;
            _site = siteService;
        }
        public async Task<IActionResult> Index()
        {

            List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
            //var SiteID = HttpContext.Session.GetInt32("SITEID");
            ResponseDTO? res = await _phoneNumber.GetAllAsync();
            if (res != null && res.IsSuccess)
            {
                phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumberDTO>>(Convert.ToString(res.Result)!);
                return View(phoneNumbers);
            }
            else
            {
                TempData["error"] = res?.Message;
            }
            return View();
        }
        // GET: PhoneNumberController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                List<SiteDTO>? siteList = new List<SiteDTO>();
                ResponseDTO? res = await _site.GetAllAsync();
                if (res != null && res.IsSuccess)
                {
                    siteList = JsonConvert.DeserializeObject<List<SiteDTO>>(Convert.ToString(res.Result)!);
                    var selectSite = siteList.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                    }).ToList();
                    ViewBag.SelectSite = selectSite;
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

        // POST: PhoneNumberController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhoneNumberDTO phoneNumberDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseDTO? res = await _phoneNumber.CreateAsync(phoneNumberDTO);
                    if (res != null && res.IsSuccess)
                    {
                        TempData["success"] = "Create New Sim Number Successfully";
                        return RedirectToAction("Index", "PhoneNumber");
                    }
                    else
                    {
                        TempData["error"] = "Create New Sim Number Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View("Create");
        }

        // GET: PhoneNumberController/Edit/5
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                List<SiteDTO>? siteList = new List<SiteDTO>();
                ResponseDTO? resSite = await _site.GetAllAsync();
                if (resSite != null && resSite.IsSuccess)
                {
                    siteList = JsonConvert.DeserializeObject<List<SiteDTO>>(Convert.ToString(resSite.Result)!);
                    var selectSite = siteList.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                    }).ToList();
                    ViewBag.SelectSite = selectSite;
                }

                ResponseDTO? res = await _phoneNumber.GetByIDAsync(Id);
                if (res != null && res.IsSuccess)
                {
                    PhoneNumberDTO? phoneNumberDTO = JsonConvert.DeserializeObject<PhoneNumberDTO>(Convert.ToString(res.Result));
                    return View(phoneNumberDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        // POST: PhoneNumberController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PhoneNumberDTO phoneNumberDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseDTO? res = await _phoneNumber.EditAsync(phoneNumberDTO);
                    if (res != null && res.IsSuccess)
                    {
                        TempData["success"] = "Edit Sim Number Successfully";
                        return RedirectToAction("Index", "PhoneNumber");
                    }
                    else
                    {
                        TempData["error"] = "Edit Sim Number Failed";
                        return View("Edit");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Edit");
            }
            return View("Index");
        }

        // GET: PhoneNumberController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ResponseDTO? res = await _phoneNumber.DeleteAsync(id);
                if (res != null && res.IsSuccess)
                {
                    PhoneNumberDTO? phoneNumberDTO = JsonConvert.DeserializeObject<PhoneNumberDTO>(Convert.ToString(res.Result));
                    TempData["success"] = "Delete Sim Number Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}

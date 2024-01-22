using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
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
        private readonly ITokenProvider _tokenProvider;
        private UserDTO _userDTO;

        public PhoneNumberController(IPhoneNumberService phoneNumber, ISiteService siteService, ITokenProvider TokenProvider)
        {
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
            _phoneNumber = phoneNumber;
            _site = siteService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.LoginUser = _userDTO;
            List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
            ResponseDTO? res = await _phoneNumber.GetListPhoneByProjectID(_userDTO.ProjectCode);
            Console.WriteLine("HERE => "+JsonConvert.SerializeObject(res));
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
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        // POST: PhoneNumberController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhoneNumberDTO phoneNumberDTO)
        {
            try
            {
                ViewBag.LoginUser = _userDTO;

                //Update SiteID
                List<SiteDTO>? siteList = new List<SiteDTO>();
                ResponseDTO? res = await _site.GetByProjectID(_userDTO.ProjectCode);
                phoneNumberDTO.SiteID = (JsonConvert.DeserializeObject<List<SiteDTO>>(res.Result.ToString()))[0].Id;
                
                //Post Model
                ResponseDTO? PostRes = await _phoneNumber.Post(phoneNumberDTO);
                if (PostRes != null && PostRes.IsSuccess)
                {
                    TempData["success"] = "Create New Device Successfully";
                    return RedirectToAction("Index", "PhoneNumber");
                }
                else
                {
                    TempData["error"] = "Create New Sim Number Failed";
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
                ViewBag.LoginUser = _userDTO;
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
                    ViewBag.LoginUser = _userDTO;
                    ResponseDTO? res = await _phoneNumber.Edit(phoneNumberDTO);
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
                ViewBag.LoginUser = _userDTO;
                ResponseDTO? res = await _phoneNumber.Delete(id);
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

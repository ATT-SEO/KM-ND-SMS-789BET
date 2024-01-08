using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FE.ADMIN.Controllers
{
    public class SMSController : Controller
    {
        private readonly ISMSService _sms;
        public SMSController(ISMSService sms)
        {
            _sms = sms;
        }
        // GET: SMSController
        public async Task<IActionResult> Index()
        {
            try
            {
                List<SMSDTO>? smsList = new List<SMSDTO>();
                ResponseDTO? res = await _sms.GetAllAsync();
                if (res != null && res.IsSuccess)
                {
                    smsList = JsonConvert.DeserializeObject<List<SMSDTO>>(Convert.ToString(res.Result)!);
                    return View(smsList);
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
        //public async Task<IActionResult> Index(int Status)
        //{
        //    try
        //    {
        //        List<SMSDTO>? smsList = new List<SMSDTO>();
        //        ResponseDTO? res = await _sms.GetByStatus(Status);
        //        if (res != null && res.IsSuccess)
        //        {
        //            smsList = JsonConvert.DeserializeObject<List<SMSDTO>>(Convert.ToString(res.Result)!);
        //            return View(smsList);
        //        }
        //        else
        //        {
        //            TempData["error"] = res?.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = ex.Message;
        //    }
        //    return View();
        //}

        // GET: SMSController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ResponseDTO? res = await _sms.GetOneSMSByID(id);
                if (res != null && res.IsSuccess)
                {
                    SMSDTO? oneSms = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(res.Result));
                    return View(oneSms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        // GET: SMSController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ResponseDTO? res = await _sms.GetOneSMSByID(id);
                if (res != null && res.IsSuccess)
                {
                    SMSDTO? oneSms = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(res.Result));
                    return View(oneSms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        // POST: SMSController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMSDTO smsDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseDTO? res = await _sms.EditAsync(smsDTO);
                    if (res != null && res.IsSuccess)
                    {
                        TempData["success"] = "Cập nhật thành công";
                        return RedirectToAction("Index", "SMS");
                    }
                    else
                    {
                        TempData["error"] = "Cập nhật thất bại";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View("Edit");
        }

        // GET: SMSController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}

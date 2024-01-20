using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FE.ADMIN.Controllers
{
    public class SMSController : Controller
    {
        private readonly ISMSService _sms;
        private readonly ITokenProvider _tokenProvider;
        private UserDTO _userDTO;
        public SMSController(ISMSService sms, ITokenProvider TokenProvider)
        {
            _sms = sms;
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
        }
        // GET: SMSController
		public async Task<IActionResult> Index(QueryParametersDTO parameters)
		{

			Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
			try
			{
				ResponseDTO? res = await _sms.GetAllAsync(parameters);
				if (res != null && res.IsSuccess)
				{
                    ViewBag.LoginUser = _userDTO;
                    var resultObject = JObject.FromObject(res.Result);
					if (resultObject.TryGetValue("data", out var data) && data != null)
					{

						var smsList = JsonConvert.DeserializeObject<List<SMSDTO>>(data.ToString());
						ViewBag.CurrentPage = parameters.Page;
						ViewBag.PageSize = parameters.PageSize;
						int totalCount = resultObject.GetValue("totalCount").Value<int>();
						int totalPages = CoreBase.CalculateTotalPages(totalCount, parameters.PageSize);
						ViewBag.TotalPages = totalPages;
						ViewBag.totalCount = totalCount;
						return View(smsList);
					}
					else
					{
						ViewBag.TotalPages = 1;
						TempData["error"] = "Danh sách LogAccount không tồn tại hoặc là null.";
					}
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


		// GET: SMSController/Details/5
		public async Task<IActionResult> Details(int id)
        {
            try
            {
                ViewBag.LoginUser = _userDTO;
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
                ViewBag.LoginUser = _userDTO;
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
                    ViewBag.LoginUser = _userDTO;
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
            ViewBag.LoginUser = _userDTO;
            return View();
        }
    }
}

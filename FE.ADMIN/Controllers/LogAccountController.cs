using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Printing;

namespace FE.ADMIN.Controllers
{
    public class LogAccountController : Controller
    {
		// GET: LogAccountController


		private readonly ILogAccountService _logAccount;
        private readonly ITokenProvider _tokenProvider;
        private UserDTO _userDTO;
        public LogAccountController(ILogAccountService logAccount, ITokenProvider TokenProvider)
		{
            _logAccount = logAccount;
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
        }

        public async Task<IActionResult> Index(QueryParametersDTO parameters)
        {

			Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
			try
            {
                ResponseDTO? res = await _logAccount.GetAllLogAccountAsync(parameters);
                if (res != null && res.IsSuccess)
                {
                    ViewBag.LoginUser = _userDTO;
                    var resultObject = JObject.FromObject(res.Result);
					if (resultObject.TryGetValue("data", out var data) && data != null)
					{

						var logAccountDTOs = JsonConvert.DeserializeObject<List<LogAccountDTO>>(data.ToString());
						ViewBag.CurrentPage = parameters.Page;
						ViewBag.PageSize = parameters.PageSize;
						int totalCount = resultObject.GetValue("totalCount").Value<int>();
						int totalPages = CoreBase.CalculateTotalPages(totalCount, parameters.PageSize);
						ViewBag.TotalPages = totalPages;
                        ViewBag.totalCount = totalCount;
						return View(logAccountDTOs);
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

        // GET: LogAccountController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        // GET: LogAccountController/Create
        public ActionResult Create()
        {
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        // POST: LogAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                ViewBag.LoginUser = _userDTO;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogAccountController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        // POST: LogAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                ViewBag.LoginUser = _userDTO;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogAccountController/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.LoginUser = _userDTO;
            return View();
        }

        // POST: LogAccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ViewBag.LoginUser = _userDTO;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
	}
}

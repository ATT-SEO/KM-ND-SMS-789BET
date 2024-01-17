using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FE.ADMIN.Controllers
{
    public class LogAccountController : Controller
    {
		// GET: LogAccountController


		private readonly ISMSService _sms;
		public LogAccountController(ISMSService sms)
		{
			_sms = sms;
		}

		public ActionResult Index()
        {
            return View();
        }

        // GET: LogAccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LogAccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
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
            return View();
        }

        // POST: LogAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
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
            return View();
        }

        // POST: LogAccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

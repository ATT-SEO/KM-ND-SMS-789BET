using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FE.ADMIN.Controllers
{
    public class SMSRawDataController : Controller
    {
        private readonly ISMSRawDataService _smsRawData;
        public SMSRawDataController(ISMSRawDataService sms)
        {
            _smsRawData = sms;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ResponseDTO _responseDTO = await _smsRawData.GetAllAsync();
                if (_responseDTO != null && _responseDTO.IsSuccess)
                {
                    List<SMSRawDataDTO> smsList = JsonConvert.DeserializeObject<List<SMSRawDataDTO>>(Convert.ToString(_responseDTO.Result.ToString())!);
                    return View(smsList);
                }
                else
                {
                    TempData["error"] = _responseDTO?.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();
        }

        
    }
}

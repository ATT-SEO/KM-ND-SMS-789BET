using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FE.ADMIN.Controllers
{
    public class SMSRawDataController : Controller
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly ISMSRawDataService _smsRawData;
        private UserDTO _userDTO;
        private readonly IPhoneNumberService _phoneNumberService;

        public SMSRawDataController(ISMSRawDataService sms, ITokenProvider TokenProvider, IPhoneNumberService phoneNumberService)
        {
            _smsRawData = sms;
            _userDTO = TokenProvider.ReadTokenClearInformation();
            _phoneNumberService = phoneNumberService;
        }

        [HttpGet]
		public async Task<IActionResult> Index(QueryParametersDTO parameters)
		{

			Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
			try
			{
                ViewBag.LoginUser = _userDTO;
				parameters.ProjectCode = _userDTO.ProjectCode;
                ResponseDTO? res = await _smsRawData.GetAllAsync(parameters);
				if (res != null && res.IsSuccess)
				{
                    var resultObject = JObject.FromObject(res.Result);
					if (resultObject.TryGetValue("data", out var data) && data != null)
					{
						var smsList = JsonConvert.DeserializeObject<List<SMSRawDataDTO>>(data.ToString());
						ViewBag.CurrentPage = parameters.Page;
						ViewBag.PageSize = parameters.PageSize;
						int totalCount = resultObject.GetValue("totalCount").Value<int>();
						int totalPages = CoreBase.CalculateTotalPages(totalCount, parameters.PageSize);
						ViewBag.TotalPages = totalPages;
						ViewBag.totalCount = totalCount;

                        List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
                        ResponseDTO? phoneDTO = await _phoneNumberService.GetListPhoneByProjectCode(_userDTO.ProjectCode);
                        if (phoneDTO != null && phoneDTO.IsSuccess)
                        {
                            phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumberDTO>>(Convert.ToString(phoneDTO.Result)!);
                            ViewBag.phoneNumbers = phoneNumbers;
                        }
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


	}
}

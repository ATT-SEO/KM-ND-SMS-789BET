using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FE.ADMIN.Controllers
{
    public class AccountRegistersController : Controller
    {
        private readonly ISMSService _sms;
        private readonly ITokenProvider _tokenProvider;
        private UserDTO _userDTO;
        private readonly IPhoneNumberService _phoneNumberService;
        public AccountRegistersController(ISMSService sms, ITokenProvider TokenProvider, IPhoneNumberService phoneNumberService)
        {
            _sms = sms;
            _tokenProvider = TokenProvider;
            _userDTO = _tokenProvider.ReadTokenClearInformation();
            _phoneNumberService = phoneNumberService;
        }
		public async Task<IActionResult> Index(QueryParametersDTO parameters)
		{

			Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
			try
			{
                ViewBag.LoginUser = _userDTO;
                parameters.ProjectCode = "bo_789bet";

                ResponseDTO? res = await _sms.GetAllAccountRegisters(parameters);
				if (res != null && res.IsSuccess)
				{
                    ViewBag.LoginUser = _userDTO;

					var smsList = JsonConvert.DeserializeObject<List<AccountRegistersDTO>>(Convert.ToString(res.Result));
					ViewBag.CurrentPage = parameters.Page;
					ViewBag.PageSize = parameters.PageSize;
					int totalCount = res.TotalCount;
					int totalPages = CoreBase.CalculateTotalPages(totalCount, parameters.PageSize);
					ViewBag.TotalPages = totalPages;
					ViewBag.totalCount = totalCount;

                    List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
                    ResponseDTO? phoneDTO = await _phoneNumberService.GetListPhoneByProjectCode("bo_789bet");
                    if (phoneDTO != null && phoneDTO.IsSuccess)
                    {
                        phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumberDTO>>(Convert.ToString(phoneDTO.Result)!);
                        ViewBag.phoneNumbers = phoneNumbers;
                    }
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
    }
}

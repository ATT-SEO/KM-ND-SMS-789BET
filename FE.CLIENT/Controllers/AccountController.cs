using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FE.CLIENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBOService _boService;
        private readonly ILogger<AccountController> _logger;
        private readonly IPhoneNumberService _phoneNumber;

        public AccountController(ILogger<AccountController> logger, IBOService boService, IPhoneNumberService phoneNumber)
        {
            _logger = logger;
            _boService = boService;
            _phoneNumber = phoneNumber;
        }

        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountRequestDTO checkAccountRequestDTO)
        {
            string Username = checkAccountRequestDTO.Account;
            object responseJson;

            if (!string.IsNullOrEmpty(Username))
            {
                ResponseDTO userAccountInfo = await _boService.BoCheckUserAccount(Username);
                ResponseAccountDTO responseJsonData = JsonConvert.DeserializeObject<ResponseAccountDTO>(Convert.ToString(userAccountInfo.Result));
                // ApiResponseDTO apiResponse = JsonConvert.DeserializeObject<ApiResponseDTO>(jsonString);
                List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
                //var SiteID = HttpContext.Session.GetInt32("SITEID");
                ResponseDTO? res = await _phoneNumber.GetListPhoneBySiteIDAsync(1);
                if (res != null && res.IsSuccess)
                {
                    phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumberDTO>>(Convert.ToString(res.Result)!);
                    
                    Random random = new Random();
                    PhoneNumberDTO randomPhoneNumber = phoneNumbers[random.Next(phoneNumbers.Count)];

                    responseJson = new
                    {
                        result = new
                        {
                            phone = "+639284718377",
                            smsCode = "KM_58_JUN88_CMD",
                            verifyCode = "+639284718377",
                            voiceSum = 0,
                            superPhone = randomPhoneNumber.Number,
                            code = 200,
                            message = "tài khoản của bạn đã đăng ký quá 24h"
                        },
                        success = true,
                        error = (string)null,
                        targetUrl = (string)null,
                        unAuthorizedRequest = false,
                        __abp = true
                    };
                    return Json(responseJson);


                }


                responseJson = new
                {
                    result = new
                    {
                        phone = "+639284718377",
                        smsCode = "Chưa setup số điện thoại nhận",
                        verifyCode = "+639284718377",
                        voiceSum = 0,
                        superPhone = "+639284718377",
                        code = 200,
                        message = "tài khoản của bạn đã đăng ký quá 24h"
                    },
                    success = true,
                    error = (string)null,
                    targetUrl = (string)null,
                    unAuthorizedRequest = false,
                    __abp = true
                };


                return Json(responseJson);
            }
            responseJson = new
            {
                result = new
                {
                    phone = (string)null,
                    smsCode = (string)null,
                    verifyCode = (string)null,
                    voiceSum = 0,
                    superPhone = (string)null,
                    code = 250,
                    message = "tài khoản của bạn đã đăng ký quá 24h"
                },
                success = true,
                error = (string)null,
                targetUrl = (string)null,
                unAuthorizedRequest = false,
                __abp = true
            };
            return Json(responseJson);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitBouns([FromBody] AccountDTO accountDTO)
        {



            return Json(new { success = true, result = "" });
        }
    }
}

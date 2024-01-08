using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FE.CLIENT.Utility;
using System.Numerics;
using System.Net.NetworkInformation;

namespace FE.CLIENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBOService _boService;
        private readonly ILogger<AccountController> _logger;
        private readonly IPhoneNumberService _phoneNumber;
        private readonly ISMSService _SMS;

        public AccountController(ILogger<AccountController> logger, IBOService boService, IPhoneNumberService phoneNumber, ISMSService SMS)
        {
            _logger = logger;
            _boService = boService;
            _phoneNumber = phoneNumber;
            _SMS = SMS;

        }

        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountRequestDTO checkAccountRequestDTO)
        {
            string Username = checkAccountRequestDTO.Account;
            object responseJson;
            Console.WriteLine(Username + "Checlklllll ");
            Console.WriteLine(!string.IsNullOrWhiteSpace(Username));
            if (!string.IsNullOrWhiteSpace(Username))
            {
                ResponseDTO userAccountInfo = await _boService.BoCheckUserAccount(Username);
                ResponseAccountDTO responseJsonData = JsonConvert.DeserializeObject<ResponseAccountDTO>(Convert.ToString(userAccountInfo.Result));
                
                if (responseJsonData != null)
                {
                    string phone = "+639284718377"; /// Số điện thoại fix cứng tạm thời

                    // ApiResponseDTO apiResponse = JsonConvert.DeserializeObject<ApiResponseDTO>(jsonString);
                    List<PhoneNumberDTO>? phoneNumbers = new List<PhoneNumberDTO>();
                    ResponseDTO? res = await _phoneNumber.GetListPhoneBySiteIDAsync(1);
                    if (res != null && res.IsSuccess)
                    {
                        phoneNumbers = JsonConvert.DeserializeObject<List<PhoneNumberDTO>>(Convert.ToString(res.Result)!);
                        Random random = new Random();
                        PhoneNumberDTO randomPhoneNumber = phoneNumbers[random.Next(phoneNumbers.Count)];

                        string smsCode = RandomString.GenerateString(9, 3, 3, 3);

                        var smsNew = new SMSDTO
                        {
                            Content = smsCode,
                            Sender = phone,
                            Device = randomPhoneNumber.Device,
                            Status = false
                        };

                        ResponseDTO? createSMS = await _SMS.CreateWebsiteAsync(smsNew);
                        if (createSMS.Result != null && createSMS.IsSuccess)
                        {
                            SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(createSMS.Result));
                            if (OneSMS.Status == true)
                            {
                                responseJson = new
                                {
                                    result = new
                                    {
                                        phone = phone,
                                        smsCode = smsCode,
                                        verifyCode = "",
                                        voiceSum = 0,
                                        status = 1,
                                        superPhone = randomPhoneNumber.Number,
                                        code = 200,
                                        message = "Đã cộng khuyến mãi. Nổ pháo hoa chúc mừng"
                                    },
                                    success = true,
                                    error = (string)null,
                                    targetUrl = (string)null,
                                    unAuthorizedRequest = false,
                                    __abp = true
                                };
                                return Json(responseJson);
                            }
                            else
                            {
                                if (OneSMS.CreatedTime != null)
                                {
                                    responseJson = new
                                    {
                                        result = new
                                        {
                                            phone = phone,
                                            smsCode = OneSMS.Content,
                                            verifyCode = "",
                                            voiceSum = 0,
                                            status = 1,
                                            superPhone = randomPhoneNumber.Number,
                                            code = 200,
                                            message = "Hệ thống đã nhận được tin nhắn khuyến mãi của bạn trước đó. Chúng tôi sẽ cập nhật trong giây lát"
                                        },
                                        success = true,
                                        error = (string)null,
                                        targetUrl = (string)null,
                                        unAuthorizedRequest = false,
                                        __abp = true
                                    };
                                    return Json(responseJson);
                                }
                                else
                                {
                                    responseJson = new
                                    {
                                        result = new
                                        {
                                            phone = phone,
                                            smsCode = smsCode,
                                            verifyCode = "+639284718377",
                                            voiceSum = 0,
                                            status = 0,
                                            superPhone = randomPhoneNumber.Number,
                                            code = 200,
                                            ///message = ""
                                        },
                                        success = true,
                                        error = (string)null,
                                        targetUrl = (string)null,
                                        unAuthorizedRequest = false,
                                        __abp = true
                                    };
                                    return Json(responseJson);
                                }
                            }
                        }
                        else
                        {
                            responseJson = new
                            {
                                result = new
                                {
                                    code = 404,
                                    message = "Lỗi hệ thống"
                                },
                                success = true,
                                error = (string)null,
                            };
                            return Json(responseJson);
                        }
                    }
                    else
                    {
                        responseJson = new
                        {
                            result = new
                            {
                                code = 250,
                                message = "Lỗi hệ thống"
                            },
                            success = true,
                            error = (string)null,
                            targetUrl = (string)null,
                            unAuthorizedRequest = false,
                            __abp = true
                        };
                        return Json(responseJson);
                    }

                }
                else
                {
                    responseJson = new
                    {
                        result = new
                        {
                            code = 250,
                            message = "Tài khoản của bạn không đủ điều kiện tham gia sự kiện này"
                        },
                        success = true,
                        error = (string)null,
                        targetUrl = (string)null,
                        unAuthorizedRequest = false,
                        __abp = true
                    };
                    return Json(responseJson);
                }
                

            }
            responseJson = new
            {
                result = new
                {
                    code = 250,
                    message = "Kiểm tra lại tài khoản của bạn"
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
            object responseJson;
            var smsCheck = new SMSDTO
            {
                Content = accountDTO.SmsCode,
                Sender = accountDTO.Phone,
            };
            ResponseDTO? checkSMS = await _SMS.CreateWebsiteAsync(smsCheck);
            if (checkSMS != null && checkSMS.IsSuccess)
            {
                SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(checkSMS.Result));
                if (OneSMS.Status == true)
                {
                    responseJson = new
                    {
                        result = new
                        {
                            code = 200,
                            message = "Đã cộng khuyến mãi. Nổ pháo hoa chúc mừng"
                        },
                        success = true,
                        __abp = true
                    };
                    return Json(responseJson);
                }
                else
                {
                    if (OneSMS.CreatedTime != null)
                    {
                        responseJson = new
                        {
                            result = new
                            {
                                code = 200,
                                message = "Hệ thống đã nhận được tin nhắn khuyến mãi của bạn. Chúng tôi sẽ cập nhật trong giây lát"
                            },
                            success = true,
                            unAuthorizedRequest = false,
                            __abp = true
                        };
                        return Json(responseJson);
                    }
                    else
                    {
                        responseJson = new
                        {
                            result = new
                            {
                                code = 200,
                                message = "Hệ thống đã nhận được tin nhắn khuyến mãi của bạn. Chúng tôi sẽ cập nhật trong giây lát"
                            },
                            success = true,
                            unAuthorizedRequest = false,
                            __abp = true
                        };
                        return Json(responseJson);
                    }
                }
            }
            responseJson = new
            {
                result = new
                {
                    code = 250,
                    message = "Vui lòng khiểm tra lại quy trình"
                },
                success = true,
                error = (string)null,
                targetUrl = (string)null,
                unAuthorizedRequest = false,
                __abp = true
            };
            return Json(responseJson);
        }
    }
}

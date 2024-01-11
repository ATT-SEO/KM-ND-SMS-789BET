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
        private readonly ILogAccountService _logAccountService;

        public AccountController(ILogger<AccountController> logger, IBOService boService, IPhoneNumberService phoneNumber, ISMSService SMS, ILogAccountService logAccountService)
        {
            _logger = logger;
            _boService = boService;
            _phoneNumber = phoneNumber;
            _SMS = SMS;
            _logAccountService = logAccountService;
        }

        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountRequestDTO checkAccountRequestDTO)
        {
            string Username = checkAccountRequestDTO.Account;
            object responseJson;

            if (!string.IsNullOrWhiteSpace(Username))
            {
                ResponseDTO userAccountInfo = await _boService.BoCheckUserAccount(Username);
                Console.WriteLine(userAccountInfo);
                ResponseAccountDTO responseJsonData = JsonConvert.DeserializeObject<ResponseAccountDTO>(Convert.ToString(userAccountInfo.Result));
                

                if (responseJsonData != null)
                {
                    try
                    {
                        string clientIPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                        var log = new LogAccountDTO
                        {
                            Account = Username,
                            FP = checkAccountRequestDTO.Regfingerprint,
                            IP = clientIPAddress,
                            Project = "KM66_MB66"
                        };

                        await _logAccountService.CreateAsync(log);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    string phone = ConvertPhoneNumber.ConvertPhone(responseJsonData.Mobile);
                    long createDateTicks = responseJsonData.CreateDate;
                    DateTime createDate = new DateTime(createDateTicks);
                    if (createDate < DateTime.Now.AddHours(-24))
                    {
                        var smsCheck = new SMSDTO
                        {
                            Account = responseJsonData.PlayerId,
                            Sender = phone
                        };
                        ResponseDTO? checkSMS = await _SMS.CheckSMSWebsite(smsCheck);
                        Console.WriteLine(checkSMS);
                        if (checkSMS.Result != null && checkSMS.IsSuccess)
                        {
                            SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(checkSMS.Result));
                            Console.WriteLine(OneSMS);
                            if (OneSMS.Status)
                            {
                                responseJson = new
                                {
                                    result = new
                                    {
                                        status = 2,
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
                                            status = 1,
                                            code = 200,
                                            message = "Hệ thống đã nhận được tin nhắn khuyến mãi của bạn. Hệ thống sẽ xử lý và cộng điểm trong giây lát"
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
                                            status = 1,
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
                        else
                        {
                            responseJson = new
                            {
                                result = new
                                {
                                    code = 250,
                                    message = "Không thể yêu cầu nhận thưởng. Tài khoản của bạn đã đăng ký quá 24h"
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
                            Account = Username,
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
                                        message = "Đã cộng khuyến mãi.Pháo hoa chúc mừng"
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
                                            verifyCode = 1,
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
                            message = "Tài khoản của bạn không tồn tại"
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
        public async Task<IActionResult> SubmitBouns([FromBody] AccountDTO accountDTO)
        {
            try
            {
				object responseJson;
				var smsCheck = new SMSDTO
				{
					Account = accountDTO.Account,
					Sender = accountDTO.Phone,
				};
				ResponseDTO? checkSMS = await _SMS.CheckSMSWebsite(smsCheck);
				if (checkSMS != null && checkSMS.IsSuccess)
				{
					SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(checkSMS.Result));
					Console.WriteLine(OneSMS);
					if (OneSMS.Status == true)
					{
						responseJson = new
						{
							result = new
							{
                                status = 2,
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
                                    status = 1,
                                    code = 200,
									message = "Hệ thống đã nhận được tin nhắn khuyến mãi của bạn. Hệ thống sẽ xử lý và cộng điểm trong giây lát"
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
                                    status = 1,
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
				
			}
            catch (Exception ex)
            {
				Console.WriteLine(ex);
			}
			object responseJson2 = new
			{
				result = new
				{
					code = 250,
					message = "Vui lòng khiểm tra lại quy trình"
				},
				error = (string)null,
				targetUrl = (string)null,
				unAuthorizedRequest = false,
				__abp = true
			};
			return Json(responseJson2);
		}
    }
}

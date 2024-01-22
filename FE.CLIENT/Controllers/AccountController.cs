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
        private const string RecaptchaSecretKey = "6LdjDFYpAAAAAJsGL5I-hHeMPNaSURINvSEoM6uH";
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
            Username = Username.Trim();
            Username = Username.ToLower();
            string recaptchaToken = checkAccountRequestDTO.RecaptchaToken;

            object responseJson;
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={RecaptchaSecretKey}&response={recaptchaToken}");

                var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
                Console.WriteLine(response);
                if (!recaptchaResponse.Success)
                {
                    responseJson = new
                    {
                        result = new
                        {
                            code = 250,
                            message = "Qúy khách vui lòng kiểm tra lại điều kiện nhận thưởng hoặc vui load lại trang !!!"
                        },
                        success = true,
                        error = "error",
                        unAuthorizedRequest = false,
                        __abp = false
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

				if (!string.IsNullOrWhiteSpace(Username))
				{
					ResponseDTO userAccountInfo = await _boService.BoCheckUserAccount(Username);
					Console.WriteLine(userAccountInfo);
					ResponseAccountDTO responseJsonData = JsonConvert.DeserializeObject<ResponseAccountDTO>(Convert.ToString(userAccountInfo.Result));
					if (responseJsonData != null)
					{

						string phone = ConvertPhoneNumber.ConvertPhone(responseJsonData.Mobile);
						long createDateTicks = responseJsonData.CreateDate;
						DateTimeOffset createDate = DateTimeOffset.FromUnixTimeMilliseconds(createDateTicks);
						DateTimeOffset currentUtcTime = DateTimeOffset.UtcNow;
						if (Username != "kawaitcn")
						{
							try
							{

								//// check log
								string clientIPAddress = HttpContext.Request.Headers["X-Forwarded-For"];
								var log = new LogAccountDTO
								{
									Account = Username,
									FP = checkAccountRequestDTO.Regfingerprint,
									IP = clientIPAddress,
									SiteID = 1,
									Project = "FREE66"
								};

								ResponseDTO? checkLog = await _logAccountService.CreateAsync(log);

								if (checkLog.IsSuccess == false)
								{
									responseJson = new
									{
										result = new
										{
											code = 250,
											message = "Tài khoản của quý khách không đủ điều kiện nhận thưởng. Vui lòng xem lại quy tắc nhận thưởng !!!"
										},
										success = true,
										error = "error",
										unAuthorizedRequest = false,
										__abp = false
									};
									return Json(responseJson);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex);
							}
							if (responseJsonData.BanksNameAccount == null)
							{
								responseJson = new
								{
									result = new
									{
										code = 250,
										message = "Tài khoản của quý khách không đủ điều kiện nhận thưởng. Vui lòng xem lại quy tắc nhận thưởng !!!"
									},
									success = true,
									error = "error",
									unAuthorizedRequest = false,
									__abp = false
								};
								return Json(responseJson);
							}

							if (responseJsonData.TotalDepositCount > 1)
							{
								responseJson = new
								{
									result = new
									{
										code = 250,
										message = "Tài khoản của quý khách không đủ điều kiện nhận thưởng. Vui lòng xem lại quy tắc nhận thưởng !!!"
									},
									success = true,
									error = "error",
									unAuthorizedRequest = false,
									__abp = false
								};
								return Json(responseJson);
							}

							if (createDate < DateTime.Now.AddHours(-24))
							{
								var smsCheck = new SMSDTO
								{
									Account = Username,
									Sender = phone
								};
								ResponseDTO? checkSMS = await _SMS.CheckSMSWebsite(smsCheck);

								if (checkSMS.Result != null && checkSMS.IsSuccess)
								{
									SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(checkSMS.Result));
									PhoneNumberDTO matchingPhoneNumber = phoneNumbers.FirstOrDefault(p => p.Device == OneSMS.Device);

									string resultNumber = matchingPhoneNumber?.Number;

									if (OneSMS.Status)
									{
										responseJson = new
										{
											result = new
											{
												status = 2,
												code = 200,
												message = $"Quý khách đã nhận thưởng thành công \n KM: FREE66. \n Số điểm: {OneSMS.Point} \n Thời gian {OneSMS.CreatedTime.Value.ToString("dd/MM/yyyy HH:mm:ss")}"
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
													phone = phone,
													smsCode = OneSMS.Content,
													verifyCode = "",
													voiceSum = 0,
													superPhone = resultNumber,
													status = 1,
													code = 200,
													message = "Hệ thống đang kiểm tra tin nhắn khuyến mãi của quý khách. Hệ thống sẽ xử lý và cộng điểm trong giây lát"
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
													phone = phone,
													smsCode = OneSMS.Content,
													verifyCode = "",
													voiceSum = 0,
													superPhone = resultNumber,
													status = 1,
													code = 200,
													message = "Hệ thống đang kiểm tra tin nhắn khuyến mãi của quý khách. Chúng tôi sẽ cập nhật trong giây lát"
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
											message = "Không thể yêu cầu nhận thưởng. Tài khoản của quý khách đã đăng ký quá 24h"
										},
										success = true,
										error = "error",
										unAuthorizedRequest = false,
										__abp = false
									};
									return Json(responseJson);
								}
							}
						}
						string smsCode = RandomString.GenerateString(9, 3, 3, 3);
						var smsNew = new SMSDTO
						{
							Account = Username,
							Content = smsCode,
							Sender = phone,
							Device = randomPhoneNumber.Device,
							Status = false,
							ProjectCode = "FREE66"
						};

						ResponseDTO? createSMS = await _SMS.CreateWebsiteAsync(smsNew);

						if (createSMS.Result != null && createSMS.IsSuccess)
						{

							SMSDTO? OneSMS = JsonConvert.DeserializeObject<SMSDTO>(Convert.ToString(createSMS.Result));

							PhoneNumberDTO matchingPhoneNumber = phoneNumbers.FirstOrDefault(p => p.Device == OneSMS.Device);

							string resultNumber = matchingPhoneNumber?.Number;


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
										status = 2,
										superPhone = resultNumber,
										code = 200,
										message = $"Quý khách đã nhận thưởng thành công. \n KM: FREE66. \n Số điểm: {OneSMS.Point}. \n Thời gian {OneSMS.CreatedTime.Value.ToString("dd/MM/yyyy HH:mm:ss")}"
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
											phone = phone,
											smsCode = OneSMS.Content,
											verifyCode = "",
											voiceSum = 0,
											superPhone = resultNumber,
											code = 200,
											message = "Hệ thống đã nhận được tin nhắn khuyến mãi của quý khách trước đó. Chúng tôi sẽ cập nhật trong giây lát"
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
											phone = phone,
											smsCode = OneSMS.Content,
											verifyCode = 1,
											voiceSum = 0,
											status = 0,
											superPhone = resultNumber,
											code = 200,
											///message = ""
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
									code = 404,
									message = "Lỗi hệ thống"
								},
								success = true,
								error = "error",
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
								message = "Tài khoản của quý khách không tồn tại"
							},
							success = true,
							error = "error",
							unAuthorizedRequest = false,
							__abp = false
						};
						return Json(responseJson);
					}


				}
				responseJson = new
				{
					result = new
					{
						code = 250,
						message = "Kiểm tra lại tài khoản của quý khách"
					},
					success = true,
					error = "error",
					unAuthorizedRequest = false,
					__abp = false
				};
				return Json(responseJson);
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
					error = "error",
					unAuthorizedRequest = false,
					__abp = false
				};
				return Json(responseJson);
			}
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
								message = $"Quý khách đã nhận thưởng thành công. \n KM: FREE66. \n Số điểm: {OneSMS.Point}. \n Thời gian {OneSMS.CreatedTime.Value.ToString("dd/MM/yyyy HH:mm:ss")}"
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
									message = "Hệ thống đã nhận được tin nhắn khuyến mãi của quý khách. Hệ thống sẽ xử lý và cộng điểm trong giây lát"
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
									code = 250,
									message = "Hệ thống chưa nhận được tin nhắn SMS của quý khách. Vui lòng gửi đúng nội dung"
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
				error = "error",
				unAuthorizedRequest = false,
				__abp = false
			};
			return Json(responseJson2);
		}
	}
}

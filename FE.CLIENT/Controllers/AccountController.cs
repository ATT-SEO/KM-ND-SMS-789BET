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
using System.Reflection;
using FE.ADMIN.Utility;

namespace FE.CLIENT.Controllers
{
	public class AccountController : Controller
    {
        private const string RecaptchaSecretKey = "6Lfrm6MpAAAAABXxe8r7X5Byy6V6LN3S4Yf44BqV";
        private readonly IBOService _boService;
        private readonly ILogger<AccountController> _logger;
        private readonly IPhoneNumberService _phoneNumber;
        private readonly ISMSService _SMS;
        private readonly ILogAccountService _logAccountService;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ResponseDTO _response;
		public AccountController(ILogger<AccountController> logger, IBOService boService, IPhoneNumberService phoneNumber, ISMSService SMS, ILogAccountService logAccountService, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_boService = boService;
			_phoneNumber = phoneNumber;
			_SMS = SMS;
			_logAccountService = logAccountService;
			_contextAccessor = httpContextAccessor;
            _response = new ResponseDTO();

        }

        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountRequestDTO checkAccountRequestDTO)
        {
            string Account = checkAccountRequestDTO.Account;
            Account = Account.Trim();
            Account = Account.ToLower();
            string recaptchaToken = checkAccountRequestDTO.RecaptchaToken;
			checkAccountRequestDTO.Token = CalculateMD5(recaptchaToken);
            checkAccountRequestDTO.IP = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            checkAccountRequestDTO.FP = checkAccountRequestDTO.Regfingerprint;

            checkAccountRequestDTO.Project = "bo_789bet"; 

            Console.WriteLine("Data IP : " + checkAccountRequestDTO.IP);
            object responseJson;
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={RecaptchaSecretKey}&response={recaptchaToken}");

                var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
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

			if (!string.IsNullOrWhiteSpace(Account))
			{
                var userAccountInfo = await _boService.BoCheckUserAccount(checkAccountRequestDTO);
                Console.WriteLine(JsonConvert.SerializeObject(userAccountInfo));
				if (userAccountInfo.IsSuccess == true)
                {
                    if(userAccountInfo.Result == null)
                    {
                        _response.IsSuccess = true;
                        _response.Code = 202;
                        _response.Message = userAccountInfo.Message;
                        return Json(_response);
                    }
                    dynamic jsonData = JsonConvert.DeserializeObject(userAccountInfo.Result.ToString());

                    Console.WriteLine("Data : " + jsonData);

                    if (jsonData != null && jsonData.ContainsKey("isSMS")) /// tồn tại là đã trả về tk đã đăng ký được rồi
                    {
						if(jsonData.status == 1)
						{
                            _response.IsSuccess = true;
                            _response.Code = 200;
							_response.Message = $"Quý khách đã nhận thưởng thành công! \n Khuyến mãi: 789BET \n Số điểm: {jsonData.point} \n Thời gian: {jsonData.createdTime.Value.ToString("HH:mm:ss")} ngày {jsonData.createdTime.Value.ToString("dd/MM/yyyy")}";
                            //_response.Result = jsonData;
							 return Json(_response);
                        }else if(jsonData.status == 0)
						{
                            _response.IsSuccess = true;
                            _response.Code = 202;
                            _response.Message = "Hệ thống đang cộng điểm thưởng. Quý khách đợi thêm giây lát !!!";
                            return Json(_response);
                        }
						else
						{
                            _response.IsSuccess = true;
                            _response.Code = 250;
                            _response.Message = "Tài khoản của quý khách không nhận được điểm thưởng. Vui lòng liên hệ bộ phận chăm sóc để được hỗ trợ !";
                            return Json(_response);
                        }
                    }
                    else
                    {
						// tài khoản chưa nhận đăng ký sms hoặc sao đó
						if(jsonData.status == false)
						{
                            _response.IsSuccess = true;
                            _response.Code = 200;
                            _response.Result = JsonConvert.SerializeObject(jsonData);
                            Console.WriteLine(jsonData.account + jsonData.projectCode + jsonData.content + jsonData.sender);

                            jsonData.VerifyCode = CalculateMD5(Convert.ToString(jsonData.account + jsonData.projectCode + jsonData.content + jsonData.sender));
                            jsonData.sender = ConverPhoneShow.FormatPhoneNumber(Convert.ToString(jsonData.sender));

                            ViewBag.DataSMS = JsonConvert.DeserializeObject<SMSDTO>(JsonConvert.SerializeObject(jsonData));  
							//JsonConvert.SerializeObject(jsonData);
                            return PartialView("_ShowSMS");
                            return Json(_response);
                        }
                    }
                }
                else
                {
                    if(userAccountInfo.Code > 9000)
                    {
                        _response.Message = userAccountInfo.Message;
                        _response.IsSuccess = false;
                        _response.Code = 403;
                        return Json(_response);
                    }
                }

            }
            _response.IsSuccess = false;
            _response.Code = 403;
            _response.Message = "Tài khoản của quý khách không đủ điều kiện nhận thưởng!";
            return Json(_response);
		}
		public async Task<IActionResult> SubmitBouns([FromBody] AccountDTO accountDTO)
		{
			try
			{
				object responseJson;
				var smsCheck = new SMSDTO
				{
					Account = accountDTO.Account,
                    VerifyCode = accountDTO.VerifyCode,
				};
				var checkSMS = await _SMS.CheckSMSWebsite(smsCheck);

                Console.WriteLine(JsonConvert.SerializeObject(checkSMS));
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
                object responseJson2 = new
                {
                    result = new
                    {
                        code = 250,
                        message = "Quý khách vui lòng khiểm tra lại quy trình"
                    },
                    error = "error",
                    unAuthorizedRequest = false,
                    __abp = false
                };
                return Json(responseJson2);

            }
			catch (Exception ex)
			{
				Console.WriteLine(ex);
                object responseJson2 = new
                {
                    result = new
                    {
                        code = 250,
                        message = "Quý khách vui lòng khiểm tra lại quy trình"
                    },
                    error = "error",
                    unAuthorizedRequest = false,
                    __abp = false
                };
                return Json(responseJson2);
            }
			
		}
        private string CalculateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}

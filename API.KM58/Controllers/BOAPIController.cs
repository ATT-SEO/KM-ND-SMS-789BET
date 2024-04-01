using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using API.KM58.Utility;
using AutoMapper;
using AutoMapper.Execution;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Reflection;

namespace API.KM58.Controllers
{
    [Route("api/BOAPI")]
    [ApiController]
    public class BOAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private readonly AppDbContext _db;
        private readonly IBOService _boService;
        private readonly ICheckConditions _checkConditions;
        private readonly IGoogleSheetService _googleSheet;
		private IMapper _mapper;
		private const string RecaptchaSecretKey = "6Lfrm6MpAAAAABXxe8r7X5Byy6V6LN3S4Yf44BqV";
		public BOAPIController(AppDbContext db, IBOService boService, IMapper mapper, ICheckConditions checkConditions, IGoogleSheetService googleSheet)
        {
            _response = new ResponseDTO();
            _db = db;
            _mapper = mapper;
            _boService = boService;
            _checkConditions = checkConditions;
            _googleSheet = googleSheet;
        }

        [HttpPost]
        [Route("CheckAccountUserName")]
        public async Task<ResponseDTO> CheckAccountUserName(LogAccountDTO logAccountDTO)
        {
            try
            {

                string _account = logAccountDTO.Account.ToString();
                string _project = logAccountDTO.Project.ToString();

                string recaptchaToken = logAccountDTO.RecaptchaToken;

                using (HttpClient httpClient = new HttpClient())
				{
					var response = await httpClient.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={RecaptchaSecretKey}&response={recaptchaToken}");

					var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
					if (!recaptchaResponse.Success)
					{
                        var log_gg_r = _googleSheet.WriteToGoogleSheets(_account, logAccountDTO.IP, logAccountDTO.FP, "Tài khoản có dấu hiệu chạy tool");
                        _response.Message = "Hệ thống đang quá tải. Quý khách vui lòng quay lại sau!!!";
						_response.IsSuccess = false;
						_response.Code = 9034;
						return _response;
					}
				}
                if (_project == "")
                {
                    _response.Message = "Hệ thông đang dừng hoạt động bảo trì. Quý khách vui lòng quay lại sau !!!";
                    _response.IsSuccess = false;
					_response.Code = 9034;
					return _response;
                }

                Site oneSite = await _db.Sites.Where(s => s.Project == _project).FirstOrDefaultAsync();
                if (oneSite == null)
                {
                    _response.Message = "Hệ thông đang dừng hoạt động bảo trì. Quý khách vui lòng quay lại sau !!!";
                    _response.IsSuccess = false;
					_response.Code = 9034;
					return _response;
                }
                Random rnd = new Random();
                int Point = rnd.Next(oneSite.MinPoint, oneSite.MaxPoint + 1); /// điểm random
                Log.Information($"KIEM TRA TAI KHOAN DIEM RANDOM {oneSite.MinPoint} ---- {oneSite.MaxPoint} || {_account} ||  {Point}");
                string AgentText = null;


				/// check trùng FP và IP
				var checkLogAccount = await _checkConditions.CheckLogAccount(logAccountDTO);
                Log.Information($"KIEM TRA TAI KHOAN 2 ||  {checkLogAccount.IsSuccess} || {checkLogAccount.Code}");
                var log_gg = _googleSheet.WriteToGoogleSheets(_account, logAccountDTO.IP, logAccountDTO.FP, checkLogAccount.Message);

				if (checkLogAccount.IsSuccess == false)
                {
                    _response.Message = "Dấu hiệu bất thường.Quý khách đã thực hiện nhiều tài khoản trong 1 thiết bị. Vui lòng xem lại hoặc liên hệ chúng tôi !!!";
                    _response.IsSuccess = false;
                    _response.Code = 9034;
                    return _response;
                }
                var jsonAll = await _boService.BOGetCheckAccount(_account);
                //JObject jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAll.Result.ToString());
                dynamic jsonResponseData = JsonConvert.DeserializeObject(jsonAll.Result.ToString());
                //string accountNumber = jsonResponseData.result.bankAccount.Accounts[0].Account;

                if ((int)jsonResponseData.status_code != 200)
                {
                    _response.Message = "Thông tin tài khoản quý khách không đúng";
                    _response.IsSuccess = false;
                    return _response;
                }
                else
                {
                    if (jsonResponseData.result == null)
                    {
                        Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG TỒN TẠI");
                        _response.Message = "Thông tin tài khoản quý khách không đúng";
                        _response.IsSuccess = false;
                        return _response;
                    }
                    _account = jsonResponseData.result?.detail?.Member?.Account;
					string MemberLevelSettingName = jsonResponseData.result?.detail?.Member?.MemberLevelSettingName;
                    if (!MemberLevelSettingName.Contains("MD-1"))
                    {
                        Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG Ở NHÓM MẶC ĐỊNH");
                        _response.Message = "Tài khoản của quý khách chưa đủ điều kiện nhận thưởng. !!!";
                        _response.IsSuccess = false;
                        _response.Code = 9032;
                        return _response;
                    }

                    // check dai ly account 
                    string Agent = jsonResponseData.result?.franchisee?.Account;
                    if(Agent != null )
                    {
                        AgentText = Agent;
						Agent oneAgent = await _db.Agents.AsNoTracking().Where(s=> s.Name == Agent && s.SiteID == oneSite.Id).FirstOrDefaultAsync();
                        if(oneAgent != null )
                        {
                            Point = rnd.Next(oneAgent.MinPoint, oneAgent.MaxPoint + 1);
							AgentText = $" {oneAgent.Name} | {oneAgent.MinPoint} - {oneAgent.MaxPoint}";
                        }
						Log.Information($"KIEM TRA ĐẠI LÝ || {_account} || {AgentText} || {Point} ĐIỂM");
					}

					// check ngan hang 
					if (oneSite.CheckBank == true)
                    {
                        var bankAccounts = jsonResponseData.result.bankAccount?.Accounts;
                        if (bankAccounts == null || bankAccounts.Count <= 0)
                        {
                            var log_gg3 = _googleSheet.WriteToGoogleSheets(_account, logAccountDTO.IP, logAccountDTO.FP, "CHƯA CẬP NHẬT NGÂN HÀNG");
                            Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG CHƯA CẬP NHẬT NGÂN HÀNG");
                            _response.Message = "Tài khoản của quý khách chưa đủ điều kiện nhận thưởng. Vui lòng cập nhật thông tin ngân hàng để được nhận thưởng !!!";
                            _response.IsSuccess = false;
                            _response.Code = 9032;
                            return _response;
                        }
                        var DepositTimes = (int)jsonResponseData.result?.transactionStatic?.DepositTimes;
                        if (DepositTimes > 0)
                        {
                            var log_gg3 = _googleSheet.WriteToGoogleSheets(_account, logAccountDTO.IP, logAccountDTO.FP, "ĐÃ NẠP TIỀN KO ĐỦ DK NHẬN");

                            Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || ĐÃ NẠP TIỀN KO ĐỦ DK NHẬN");
                            _response.Message = "Tài khoản của quý khách đã thực hiện nạp tiền. Vui lòng xem lại điều kiện nhận thưởng. Cảm ơn quý khách !!!";
                            _response.IsSuccess = false;
                            _response.Code = 9032;
                            return _response;
                        }
                    }
                    if (oneSite.CheckSMS == true)
                    {
                        if (jsonResponseData.result.detail?.Member?.Mobile == null)
                        {
                            var log_gg3 = _googleSheet.WriteToGoogleSheets(_account, logAccountDTO.IP, logAccountDTO.FP, "KHÔNG CHƯA CẬP NHẬT SỐ ĐIỆN THOẠI");
                            Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG CHƯA CẬP NHẬT SỐ ĐIỆN THOẠI");
                            _response.Message = "Tài khoản của quý khách chưa đủ điều kiện nhận thưởng. Vui lòng cập nhật thêm thông tin liên hệ ( số điện thoại )";
                            _response.IsSuccess = false;
                            _response.Code = 9033;
                            return _response;
                        }
                        else
                        {
                            AccountRegisters oneCheckAccount = await _db.AccountRegisters.Where(a => a.ProjectCode == oneSite.Project && a.Account == _account).FirstOrDefaultAsync();
                            if (oneCheckAccount != null)
                            {
                                if (oneCheckAccount.Status == 1)
                                {
                                    _response.IsSuccess = true;
                                    _response.Message = "Tài khoản của quý khách đã nhận thành công khuyến mãi này.";
                                    _response.Code = 200;
                                    _response.Result = _mapper.Map<AccountRegisters>(oneCheckAccount);
                                }
                                else if (oneCheckAccount.Status == 0)
                                {
                                    _response.IsSuccess = true;
                                    _response.Message = "Tài khoản của quý khách đang trong trạng thái chờ xử lý.";
                                    _response.Code = 200;
                                }
                                else
                                {
                                    _response.IsSuccess = false;
                                    _response.Message = "Tài khoản của quý khách không đủ điều kiện nhận khuyến mãi này. Vui lòng tham khảo các khuyến mãi khác.";
                                    _response.Code = 9035;
                                }
                                return _response;
                            }

                            List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Where(p => p.SiteID == oneSite.Id && p.Status == true).ToListAsync();
                            if (phoneNumbers.Count <= 0)
                            {
                                _response.Message = "Hệ thống nhận khuyến mãi qua SMS chưa sẵn sàng !!!";
                                _response.IsSuccess = false;
                                _response.Code = 9035;
                                return _response;
                            }
                            Random random2 = new Random();
                            List<PhoneNumberDTO>? phoneNumberDTO = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
                            PhoneNumberDTO randomPhoneNumber = phoneNumberDTO[random2.Next(phoneNumberDTO.Count)];
                            var SmsDTO = new SMSDTO
                            {
                                Account = _account,
                                Content = RandomString.GenerateString(10, 3, 4, 3),
                                Sender = ConvertPhoneNumber.ConvertPhone(Convert.ToString(jsonResponseData.result.detail?.Member?.Mobile)),
                                ProjectCode = oneSite.Project,
                                Device = randomPhoneNumber.Device,
                                PhoneReceive = randomPhoneNumber.Number,
                                AutoPoint = oneSite.AutoPoint,
                                Status = false,
                                FP = logAccountDTO.FP,
                                IP = logAccountDTO.IP,
								AgentText = AgentText, 
								CreatedTime = DateTime.Now,
                                UpdatedTime = DateTime.Now,
                            };
                            var checkAccountSMS = await _checkConditions.CheckAccountSMS(SmsDTO); // vừa check vừa tạo mới Account đăng ký KM
							return checkAccountSMS;
                        }
                    }
                    AccountRegistersDTO accountRegistersDTO = new AccountRegistersDTO();
                    accountRegistersDTO.Account = _account;
                    accountRegistersDTO.Status = 0;
                    accountRegistersDTO.isSMS = false;
                    accountRegistersDTO.Point = Point;
                    accountRegistersDTO.AgentText = AgentText;
                    accountRegistersDTO.AutoPoint = oneSite.AutoPoint;
                    accountRegistersDTO.ProjectCode = oneSite.Project;
                    accountRegistersDTO.FP = logAccountDTO.FP;
                    accountRegistersDTO.IP = logAccountDTO.IP;

                    var CheckAccountRegisters = await _checkConditions.CheckAccountRegisters(accountRegistersDTO, oneSite); // vừa check vừa tạo mới Account đăng ký KM
                    Console.WriteLine("logggg account add - " + JsonConvert.SerializeObject(CheckAccountRegisters));
                    return CheckAccountRegisters;
                    //_response.Result = resultObject;
                    _response.IsSuccess = true;
                    _response.Message = "Hệ thống sẽ xét duyệt và tiến hành xử lý. Vui lòng theo dõi hòm thư nội bộ hoặc kiểm tra lại sau ít phút ";
                    _response.Code = 200;
                }
        }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
                _response.Code = 500;
            }
            return _response;
        }

        [HttpGet]
        [Route("CheckAccountCMDUserName/{Account}")]
        public async Task<ResponseDTO> CheckAccountCMDUserName(string Account)
        {
            try
            {
                string _account = Account;
                var jsonAllJiLiFishTickets = await _boService.BOGetCheckAccountCMD(_account);
                JObject jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                JObject resultObject = jsonResponseData["result"] as JObject;
                _response.Result = resultObject;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

    }
}

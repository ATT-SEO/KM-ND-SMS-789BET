using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using API.KM58.Utility;
using AutoMapper;
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
        private IMapper _mapper;
        public BOAPIController(AppDbContext db, IBOService boService, IMapper mapper, ICheckConditions checkConditions)
        {
            _response = new ResponseDTO();
            _db = db;
            _mapper = mapper;
            _boService = boService;
            _checkConditions = checkConditions;

        }

        [HttpPost]
        [Route("CheckAccountUserName")]
        public async Task<ResponseDTO> CheckAccountUserName(LogAccountDTO logAccountDTO)
        {
            try
            {
                string _account = logAccountDTO.Account.ToString();
                string _project = logAccountDTO.Project.ToString();
                if (_project == "")
                {
                    _response.Message = "Hệ thông đang dừng hoạt động bảo trì. Quý khách vui lòng quay lại sau !!!";
                    _response.IsSuccess = false;
                    return _response;
                }

                Site oneSite = await _db.Sites.Where(s => s.Project == _project).FirstOrDefaultAsync();
                if (oneSite == null)
                {
                    _response.Message = "Hệ thông đang dừng hoạt động bảo trì. Quý khách vui lòng quay lại sau !!!";
                    _response.IsSuccess = false;
                    return _response;
                }
                Random rnd = new Random();
                int Point = rnd.Next(oneSite.MinPoint, oneSite.MaxPoint + 1); /// điểm random
                                                                              /// check trùng FP và IP
                var checkLogAccount = await _checkConditions.CheckLogAccount(logAccountDTO);
                if (checkLogAccount.Code == 404 || checkLogAccount.IsSuccess == false)
                {
                    _response.Message = "Dấu hiệu bất thường.Bạn đã thực hiện nhiều tài khoản trong 1 thiết bị. Vui lòng xem lại hoặc liên hệ chúng tôi !!!";
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
                    _response.Message = "Thông tin tài khoản không đúng";
                    _response.IsSuccess = false;
                    return _response;
                }
                else
                {
                    if (jsonResponseData.result == null)
                    {
                        Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG TỒN TẠI");
                        _response.Message = "Thông tin tài khoản không đúng";
                        _response.IsSuccess = false;
                        return _response;
                    }
                    if (oneSite.CheckBank == true)
                    {
                        var bankAccounts = jsonResponseData.result.bankAccount?.Accounts;
                        if (bankAccounts == null || bankAccounts.Count <= 0)
                        {
                            Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG CHƯA CẬP NHẬT NGÂN HÀNG");
                            _response.Message = "Tài khoản của bạn chưa đủ điều kiện nhận thưởng. Vui lòng cập nhật thông tin ngân hàng để được nhận thưởng !!!";
                            _response.IsSuccess = false;
                            _response.Code = 9032;
                            return _response;
                        }
                    }
                    if (oneSite.CheckSMS == true)
                    {
                        if (jsonResponseData.result.detail?.Member?.Mobile == null)
                        {
                            Log.Information($"KIEM TRA TAI KHOAN || {_account} || {_project} || KHÔNG CHƯA CẬP NHẬT SỐ ĐIỆN THOẠI");
                            _response.Message = "Tài khoản của bạn chưa đủ điều kiện nhận thưởng. Vui lòng cập nhật thêm thông tin liên hệ ( số điện thoại )";
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
                                    _response.Message = "Tài khoản đã nhận thành công khuyến mãi này.";
                                    _response.Code = 200;
                                    _response.Result = _mapper.Map<AccountRegisters>(oneCheckAccount);
                                }
                                else if (oneCheckAccount.Status == 0)
                                {
                                    _response.IsSuccess = true;
                                    _response.Message = "Tài khoản đang trong trạng thái chờ xử lý.";
                                    _response.Code = 200;
                                }
                                else
                                {
                                    _response.IsSuccess = false;
                                    _response.Message = "Tài khoản bị từ chối .";
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

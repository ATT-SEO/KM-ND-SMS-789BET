using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using API.KM58.Utility;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Reflection;
using System.Security.Cryptography;

namespace API.KM58.Service
{
    
    public class CheckConditions : ICheckConditions
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        private IBOService _boService;
        public CheckConditions(AppDbContext db, IMapper mapper, IBOService boService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
            _boService = boService;
        }
        //201 Created: Tài nguyên đã được tạo thành công.
        public async Task<ResponseDTO?> CheckAccountRegisters(AccountRegistersDTO accountRegistersDTO, Site oneSite)
        {
            try
            {
                AccountRegisters registers = await _db.AccountRegisters.FirstOrDefaultAsync(s => s.Account == accountRegistersDTO.Account);
                if (registers != null)
                {
                    if (registers.Status == 1)
                    {
                        _response.IsSuccess = true;
                        _response.Message = "Tài khoản của quý khách đã được cộng điểm thành công.";
                        _response.Code = 200;
                        _response.Result = _mapper.Map<AccountRegisters>(registers);
                        Log.Information($"SUCCESS SEVICE CheckAccountRegisters CỘNG ĐIỂM || {registers.Account} || Điểm: {registers.Point} || USER : {registers.UserPoint} || {registers.ProjectCode}");
                    }
                    else if(registers.Status == 0){
                        _response.IsSuccess = true;
                        _response.Message = "Tài khoản của quý khách đang trong trạng thái chờ xử lý.";
                        _response.Code = 200;
                        Log.Information($"SEVICE CheckAccountRegisters CHỜ CỘNG ĐIỂM || {registers.Account} || {registers.ProjectCode}");
                   }else
                   {
                        _response.IsSuccess = false;
                        _response.Message = "Tài khoản của quý khách không đủ điều kiện nhận khuyến mãi này. Vui lòng tham khảo các khuyến mãi khác.";
                        Log.Information($"SEVICE CheckAccountRegisters TỪ CHỐI CỘNG ĐIỂM || {registers.Account} || {registers.ProjectCode}");
                    }
                }
                else
                {
                    accountRegistersDTO.CreatedTime = DateTime.Now;
                    accountRegistersDTO.UpdatedTime = DateTime.Now;
                    accountRegistersDTO.Audit = oneSite.Audit;
                    string createdTimeAsString = accountRegistersDTO.CreatedTime.ToString();
                    accountRegistersDTO.Token = RandomString.ComputeSHA1Hash(accountRegistersDTO.Account + createdTimeAsString);
                    AccountRegisters createRegisters = _mapper.Map<AccountRegisters>(accountRegistersDTO);
                    if (accountRegistersDTO.AutoPoint == true)
                    {
                        createRegisters.AutoPoint = true;
                        // cộng điểm thẳng lên BO Nếu đang là cộng điểm tự động
                        if (oneSite.Project == "bo_789bet")
                        {
                            var addPointBO = await _boService.addPointBo789BET(oneSite.Project, accountRegistersDTO.Account, accountRegistersDTO.Point, oneSite.Round , oneSite.Remarks, oneSite.Ecremarks);
                            Log.Information($"CONG TU DONG LEN BO " + addPointBO.IsSuccess);

                            if (addPointBO.IsSuccess == false)
                            {
                                Log.Information($"ERROR SEVICE CheckAccountRegisters BO || {accountRegistersDTO?.Account} || {accountRegistersDTO?.ProjectCode} || LOI CONG TU DONG LEN BO");
                                var saveAddPointBO = await _boService.savePointBoAuto789BET(accountRegistersDTO, oneSite, false);
                                _response.IsSuccess = false;
                                _response.Code = 404;
                                _response.Message = "Đăng ký Nhận điểm thưởng không thành công. Vui lòng liên hệ bộ phận chăm sóc để được giải đáp.";
                                return _response;
                            }else {
                                Log.Information($"CỘNG TỰ ĐỘNG THÀNH CÔNG || {accountRegistersDTO.Account} || {accountRegistersDTO.Point} ĐIỂM || {accountRegistersDTO.ProjectCode}");
                                var saveAddPointBO = await _boService.savePointBoAuto789BET(accountRegistersDTO, oneSite);
                                createRegisters.UserPoint = "System";
                                createRegisters.handler = "System";
                                createRegisters.Status = 1;
                                _response.Code = 200;
                                _response.Message = "Nhận thưởng thành công.";
                            }
                        }
                    }else
                    {
                        var addHandApprove = await _boService.handApproveAccount(accountRegistersDTO, oneSite);
                        if(addHandApprove.IsSuccess == false)
                        {
                            Log.Information($"ERROR SEVICE GỬI DUYỆT TAY THẤT BẠI || {accountRegistersDTO.Account} || {accountRegistersDTO.ProjectCode}");
                            _response.IsSuccess = false;
                            _response.Message = addHandApprove.Message;
                            _response.Code = 500;
                            return _response;
                        }
                        Log.Information($"SUCCESS SEVICE GỬI DUYỆT TAY THÀNH CÔNG || {accountRegistersDTO.Account} || {accountRegistersDTO.ProjectCode}");
                        createRegisters.Status = 0;
                        _response.Message = "Đăng ký nhận khuyến mãi thành công. Vui lòng chờ hệ thống xử lý.";
                        _response.Code = 201;
                    }
                    await _db.AccountRegisters.AddAsync(createRegisters);
                    await _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<AccountRegisters>(createRegisters);
                    _response.IsSuccess = true;
                    Log.Information($"SEVICE CheckAccountRegisters TẠO MỚI || {accountRegistersDTO.Account} || {accountRegistersDTO.ProjectCode}");
                    return _response;

                }
            }
            catch (Exception ex)
            {
                Log.Information($"ERROR SEVICE CheckAccountRegisters || {accountRegistersDTO.Account} || {accountRegistersDTO.ProjectCode} ||  aaa");
                _response.IsSuccess = false;
                _response.Message =  ex.Message;
                _response.Code = 500;
            }
            return _response;
        }
        public async Task<ResponseDTO?> CheckAccountSMS(SMSDTO smsDTO)
        {
            try
            {
                string comparisonPart = smsDTO.Sender.Substring(3);
                Console.WriteLine(comparisonPart);
                SMS existingSMS = await _db.SMS.FirstOrDefaultAsync(s => (s.Sender == smsDTO.Sender) || (s.Sender.EndsWith(comparisonPart)));
                if (existingSMS == null)
                {
                    smsDTO.SiteTime = DateTime.Now;
                    SMS newSMS = _mapper.Map<SMS>(smsDTO);
                    _db.SMS.Add(newSMS);
                    await _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<SMS>(newSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Thêm mới SMS thành công.";
                    _response.Code = 201;
                    Log.Information($"SUCCESS SEVICE CheckAccountSMS || {smsDTO.Account} || {smsDTO.Sender} || {smsDTO.Content}");
                }
                else
                {
                    _response.Result = _mapper.Map<SMSDTO>(existingSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Đã có SMS ở hệ CheckAccountSMS.";
                    _response.Code = 200;
                    Log.Information($"SEVICE CheckAccountSMS TỒN TẠI || {smsDTO.Account} || {smsDTO.Sender}");
                }
            }
            catch (Exception ex)
            {
                Log.Information($"ERROR SEVICE CheckAccountSMS || {smsDTO.Account} || {smsDTO.Content} || SĐT KHÁCH:{smsDTO.Sender} || {ex.Message}");
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDTO?> CheckLogAccount(LogAccountDTO logAccountDTO)
        {
            try
            {
                ///check token với captcha gg -> md5(token) để làm ít lại data.


                LogAccount logAccount1 = await _db.LogAccounts.FirstOrDefaultAsync(s => (s.IP == logAccountDTO.IP && s.FP == logAccountDTO.FP) && s.Project == logAccountDTO.Project);
                if (logAccount1 == null)
                {
                    logAccountDTO.CreatedTime = DateTime.Now;
                    LogAccount logAccount = _mapper.Map<LogAccount>(logAccountDTO);
                    _db.LogAccounts.Add(logAccount);
                    _db.SaveChanges();
                    _response.IsSuccess = true;
                    _response.Code = 201;
                    Log.Information($"SUCCESS SEVICE CheckLogAccount || {logAccountDTO.Account} || {logAccountDTO.Project} || IP:{logAccountDTO.IP} || FP:{logAccountDTO.FP}");
                    return _response;
                }
                else
                {
                    AccountRegisters logAccount2 = await _db.AccountRegisters.FirstOrDefaultAsync(s => (s.IP == logAccountDTO.IP && s.FP == logAccountDTO.FP) && s.Status == 1 && s.ProjectCode == logAccountDTO.Project);
                    if(logAccount2 != null)
                    {
                        if (logAccount2.Account != logAccountDTO.Account)
                        {
                            Log.Information($"ERROR SEVICE CheckLogAccount LẠM DỤNG || {logAccountDTO.Account} || {logAccountDTO.Project} || IP:{logAccountDTO.IP} || FP:{logAccountDTO.FP}");
                            _response.IsSuccess = false;
                            _response.Message = "Dấu hiệu bất thường.";
                            return _response;
                        }
                    }
                   
                    Log.Information($"SUCCESS SEVICE CheckLogAccount CHECK N || {logAccountDTO.Account} || {logAccountDTO.Project} || IP:{logAccountDTO.IP} || FP:{logAccountDTO.FP}");
                    _response.IsSuccess = true;
                    _response.Code = 200;
                    _response.Result = _mapper.Map<LogAccount>(logAccountDTO);
                    return _response;

                }
            }
            catch (Exception ex)
            {
                Log.Information($"ERROR SEVICE CheckLogAccount || {logAccountDTO.Account} || {logAccountDTO.Project} || {ex.Message}");
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
            
            return _response;
        }

        public async Task<ResponseDTO?> CheckOneAccountRegisters(string Account, string Project)
        {
            AccountRegisters registers = await _db.AccountRegisters.FirstOrDefaultAsync(s => s.Account == Account && s.ProjectCode == Project);
            if (registers != null)
            {
                if (registers.Status == 1)
                {
                    _response.IsSuccess = true;
                    _response.Message = "Tài khoản đã được cộng điểm thành công.";
                    _response.Code = 200;
                    _response.Result = _mapper.Map<AccountRegisters>(registers);
                    Log.Information($"SUCCESS SEVICE CheckOneAccountRegisters CỘNG ĐIỂM || {registers.Account} || Điểm: {registers.Point} || USER : {registers.UserPoint} || {registers.ProjectCode}");
                }
                else if (registers.Status == 0)
                {
                    _response.IsSuccess = true;
                    _response.Message = "Tài khoản đang trong trạng thái chờ xử lý.";
                    _response.Code = 200;
                    Log.Information($"SEVICE CheckOneAccountRegisters CHỜ CỘNG ĐIỂM || {registers.Account} || {registers.ProjectCode}");
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Tài khoản bị từ chối .";
                    Log.Information($"SEVICE CheckOneAccountRegisters TỪ CHỐI CỘNG ĐIỂM || {registers.Account} || {registers.ProjectCode}");
                }
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Tài khoản chưa có trong chương trình khuyến mãi.";
                _response.Code = 403;
                Log.Information($"SEVICE CheckOneAccountRegisters NO-DATABASE  || {Account} || ");
            }
            return _response;
        }

        public async Task<ResponseDTO?> CheckOneAccountSMS(string Account)
        {
            try
            {
                SMS existingSMS = await _db.SMS.FirstOrDefaultAsync(s => s.Account == Account);
                if (existingSMS == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Tài khoản chưa có trong chương trình khuyến mãi.";
                    _response.Code = 403;
                    Log.Information($"SUCCESS SEVICE CheckOneAccountSMS || {Account} ");
                }
                else
                {
                    _response.Result = _mapper.Map<SMSDTO>(existingSMS);
                    _response.IsSuccess = true;
                    _response.Code = 200;
                    _response.Message = "Đã có SMS ở hệ thống.";
                    Log.Information($"SEVICE CheckOneAccountSMS TỒN TẠI || {Account}");
                }
            }
            catch (Exception ex)
            {
                Log.Information($"ERROR SEVICE CheckOneAccountSMS || {Account} || {ex.Message}");
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }
    }
}

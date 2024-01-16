using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace API.KM58.Controllers
{
    [Route("api/SMS")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly IBOService _boService;

        public SMSController(AppDbContext db, IMapper mapper, IBOService boService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
            _boService = boService;

        }

        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                List<SMS> SMSList = await _db.SMS.ToListAsync();
                _response.Result = _mapper.Map<List<SMSDTO>>(SMSList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByStatus/{Status}")]
        public async Task<ResponseDTO> GetByStatus(int Status)
        {
            bool status = false;
            if(Status == 1)
            {
                status = true;
            }
            try
            {
                List<SMS> SMSList = await _db.SMS.Where(s => s.Status == status).ToListAsync();
                _response.Result = _mapper.Map<List<SMSDTO>>(SMSList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetDateTimeEnd/{Device}")]
        public async Task<ResponseDTO> GetDateTimeEnd(string Device)
        {
            try
            {
                SMS latestSms = await _db.SMS.Where(s => s.Device == Device)
                    .OrderByDescending(s => s.CreatedTime)
                    .FirstOrDefaultAsync();

                _response.Result = _mapper.Map<SMSDTO>(latestSms);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetTotalSMS/")]
        public async Task<ResponseDTO> GetTotalSMS([FromQuery] int Total, [FromQuery] string Device)

        {
            try
            {
                IEnumerable<SMS> listSMS = _db.SMS
                .Where(s => s.Device == Device && s.CreatedTime == null)
                .OrderByDescending(w => w.SiteTime)
                .Take(Total)
                .ToList();
                _response.Result = _mapper.Map<List<SMSDTO>>(listSMS);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }



        [HttpGet]
        [Route("{Id}")]
        public async Task<ResponseDTO> Get(int Id)
        {
            try
            {
                SMS SMSList = await _db.SMS.Where(x => x.Id == Id).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<SMSDTO>(SMSList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post(SMSDTO inputSMS)
        {
            try
            {
                string comparisonPart = inputSMS.Sender.Substring(3);
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s => s.Device == inputSMS.Device 
                    && s.Content == inputSMS.Content
                    && s.Sender.EndsWith(comparisonPart));

                if (existingSMS != null && existingSMS.CreatedTime == null)
                {
                    Site site = _db.Sites.First(u => u.Name == inputSMS.ProjectCode || u.Project == inputSMS.ProjectCode);

                    Random rnd = new Random();
                    int Point = rnd.Next(site.MinPoint, site.MaxPoint + 1);
                    string Site = site.Name;
                    string Account = existingSMS.Account;
                    int Round = site.Round;
                    string Remarks = site.Remarks;
                    string Ecremarks = site.Ecremarks;

                    JObject jsonResponseData;

                    if (Site.Trim() == "mocbai")
                    {
                        var jsonAllJiLiFishTickets = await _boService.addPointClient(Site, Account, Point, Round, Remarks, Ecremarks);
                        jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());

                    }
                    else
                    {
                        var jsonAllJiLiFishTickets = await _boService.addPointClientCMD(Site, Account, Point, Round, Remarks, Ecremarks);
                        jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());
                    }

                    if (jsonResponseData != null && jsonResponseData["status_code"] != null && (int)jsonResponseData["status_code"] == 200)
                    {
                        existingSMS.CreatedTime = DateTime.Now;
                        existingSMS.ProjectCode = inputSMS.ProjectCode;
                        existingSMS.Point = Point;
                        existingSMS.Status = true;
                        _db.Entry(existingSMS).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                        _response.Result = existingSMS;
                        _response.IsSuccess = true;
                        _response.Message = "Cập nhật điểm thành công.";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Lỗi cộng điểm vào bo";
                    }
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "SMS không hợp lệ cơ sở dữ liệu.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Route("PostWebsite")]
        public async Task<ResponseDTO> PostWebsite(SMSDTO inputSMS)
        {
            try
            {
                string comparisonPart = inputSMS.Sender.Substring(3);
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s =>(s.Sender == inputSMS.Sender) || (s.Sender.EndsWith(comparisonPart)));
                if (existingSMS == null)
                {
                    inputSMS.SiteTime = DateTime.Now;
                    SMS newSMS = _mapper.Map<SMS>(inputSMS);
                    _db.SMS.Add(newSMS);
                    await _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<SMS>(newSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Thêm mới SMS thành công.";
                }
                else
                {
                    _response.Result = _mapper.Map<SMSDTO>(existingSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Đã có SMS ở hệ thống.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Route("CheckSMSWebsite")]
        public async Task<ResponseDTO> CheckSMSWebsite(SMSDTO inputSMS)
        {
            try
            {
                string comparisonPart = inputSMS.Sender.Substring(3);
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s => (s.Account == inputSMS.Account && s.Sender == inputSMS.Sender) ||
        (s.Account == inputSMS.Account && s.Sender.EndsWith(comparisonPart)));

                if (existingSMS != null)
                {
                    _response.Result = _mapper.Map<SMS>(existingSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Kiểm tra thành công.";
                }
                else
                {
                    _response.Result = null;
                    _response.IsSuccess = true;
                    _response.Message = "Thông tin chưa có trên hệ thống.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<ResponseDTO> Delete(int Id)
        {
            try
            {
                SMS _SMS = await _db.SMS.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (_SMS != null)
                {
                    _db.SMS.Remove(_SMS);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.InnerException.Message;
            }
            return _response;
        }
    }
}

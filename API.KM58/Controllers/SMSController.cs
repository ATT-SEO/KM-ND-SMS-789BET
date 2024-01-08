using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public SMSController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
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
                .Where(s => s.Device == Device)
                .OrderByDescending(w => w.CreatedTime)
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
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s => s.Device == inputSMS.Device 
                    && s.Content == inputSMS.Content 
                    && s.Sender == inputSMS.Sender);

                if (existingSMS != null && existingSMS.CreatedTime == null)
                {
                    inputSMS.CreatedTime = DateTime.Now; // dựa vào CreatedTime là từ app update
                    //SMS newSMS = _mapper.Map<SMS>(inputSMS);
                    //_db.SMS.Add(newSMS);
                    //await _db.SaveChangesAsync();

                    //_response.IsSuccess = true;
                    //_response.Message = "Thêm mới SMS thành công.";
                    _db.Entry(existingSMS).State = EntityState.Modified;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Cập nhật thành công.";


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
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s =>s.Sender == inputSMS.Sender);
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
                SMS existingSMS = await _db.SMS
                    .FirstOrDefaultAsync(s => s.Content == inputSMS.Content&& s.Sender == inputSMS.Sender);

                if (existingSMS != null)
                {
                    _response.Result = _mapper.Map<SMS>(existingSMS);
                    _response.IsSuccess = true;
                    _response.Message = "Thêm mới SMS thành công.";
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

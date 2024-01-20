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
    [Route("api/SMSRawData")]
    [ApiController]
    public class SMSRawDataController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly IBOService _boService;

        public SMSRawDataController(AppDbContext db, IMapper mapper, IBOService boService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
            _boService = boService;
        }

        [HttpGet]
        public ResponseDTO Get([FromQuery] QueryParametersDTO parameters)
        {
            try
            {
                var query = _db.SMSRawData.AsQueryable();
                if (!string.IsNullOrEmpty(parameters.Sender))
                {
                    query = query.Where(w => w.Sender == parameters.Sender);
                }
                if (!string.IsNullOrEmpty(parameters.Content))
                {
                    query = query.Where(w => w.Content == parameters.Content);
                }
                if (parameters.SeachStatus.HasValue)
                {
                    if (parameters.SeachStatus == 1)
                    {
                        query = query.Where(w => w.Status == true);
                    }
                    else if (parameters.SeachStatus == 9)
                    {
                        query = query.Where(w => w.Status == false);
                    }
                }
                if (!string.IsNullOrEmpty(parameters.ProjectCode))
                {
                    query = query.Where(w => w.ProjectID == parameters.ProjectCode);
                }
                if (!string.IsNullOrEmpty(parameters.Device))
                {
                    query = query.Where(w => w.Device == parameters.Device);
                }
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    if (string.IsNullOrEmpty(parameters.SortDirection) || parameters.SortDirection.ToLower() == "asc")
                    {
                        query = query.OrderBy(w => EF.Property<object>(w, parameters.SortBy));
                    }
                    else
                    {
                        query = query.OrderByDescending(w => EF.Property<object>(w, parameters.SortBy));
                    }
                }
                else
                {
                    query = query.OrderByDescending(w => w.Id);
                }
                int skipCount = (parameters.Page - 1) * parameters.PageSize;
                IEnumerable<SMSRawData> smsRawDatas = query
                    .Skip(skipCount)
                    .Take(parameters.PageSize)
                    .ToList();
                int totalCount = query.Count();
                var result = new
                {
                    Data = _mapper.Map<IEnumerable<SMSRawDataDTO>>(smsRawDatas),
                    TotalCount = totalCount
                };

                _response.Result = result;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }

            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post(SMSRawDataDTO inputSMSDTO)
        {
            try
            {
                SMSRawData postSMSData = _mapper.Map<SMSRawData>(inputSMSDTO);
                postSMSData.CreatedTime = DateTime.Now;
                _db.SMSRawData.Add(postSMSData);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetTotalSMS/")]
        public async Task<ResponseDTO> GetTotalSMS(int Total, string Device)
        {
            try
            {
                List<SMSRawData> listSMS = _db.SMSRawData.Where(s => s.Device == Device)
                .OrderByDescending(w => w.CreatedTime)
                .Take(Total)
                .ToList();
                _response.Result = _mapper.Map<List<SMSRawDataDTO>>(listSMS);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<ResponseDTO> Delete(int Id)
        {
            try
            {
                SMSRawData _SMS = await _db.SMSRawData.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (_SMS != null)
                {
                    _db.SMSRawData.Remove(_SMS);
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

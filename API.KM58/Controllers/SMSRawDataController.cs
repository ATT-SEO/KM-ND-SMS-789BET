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
        public async Task<ResponseDTO> Get()
        {
            try
            {
                List<SMSRawData> SMSList = await _db.SMSRawData.OrderByDescending(sms => sms.CreatedTime).ToListAsync();
                _response.Result = _mapper.Map<List<SMSRawDataDTO>>(SMSList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
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

    }
}

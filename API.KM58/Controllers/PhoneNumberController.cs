using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/PhoneNumber")]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        public PhoneNumberController(AppDbContext db, IMapper mapper)
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
                List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Include(u => u.Site).ToListAsync();
                _response.Result = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
                _response.Code = 200;
                _response.TotalCount = phoneNumbers.Count;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDTO> Get(int id)
        {
            try
            {
                PhoneNumber phoneNumber = await _db.PhoneNumbers.Include(u => u.Site).Where(s => s.Id == id).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<PhoneNumberDTO>(phoneNumber);
                _response.Code = 200;
                _response.TotalCount = 1;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByNumber/{Number}")]
        public async Task<ResponseDTO> GetByNumber(string Number)
        {
            try
            {
                PhoneNumber phoneNumber = await _db.PhoneNumbers.Include(u => u.Site).Where(s => s.Status == true && s.Number == Number).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<PhoneNumberDTO>(phoneNumber);
                _response.Code = 200;
                _response.TotalCount = 1;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetListPhoneBySiteID/{SiteID}")]
        public async Task<ResponseDTO> GetListPhoneBySiteID(int SiteID)
        {
            try
            {
                List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Include(u => u.Site).Where(u => u.SiteID == SiteID).ToListAsync();
                _response.Result = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
                _response.Code = 200;
                _response.TotalCount = phoneNumbers.Count;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetListPhoneByProjectCode/{ProjectCode}")]
        public async Task<ResponseDTO> GetListPhoneByProjectCode(string ProjectCode)
        {
            try
            {
                Console.WriteLine(ProjectCode + "*****************");
                List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Include(u => u.Site).Where(u => u.Site.Project == ProjectCode).ToListAsync();
                _response.Result = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
                _response.Code = 200;
                _response.TotalCount = phoneNumbers.Count;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] PhoneNumber phoneNumberDTO)
        {
            try
            {
                phoneNumberDTO.CreatedTime = DateTime.Now;
                phoneNumberDTO.UpdatedTime = DateTime.Now;
                PhoneNumber phoneNumber = _mapper.Map<PhoneNumber>(phoneNumberDTO);
                _db.PhoneNumbers.Add(phoneNumberDTO);
                _db.SaveChanges();
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
                _response.Code = 200;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public async Task<ResponseDTO> Put([FromBody] PhoneNumber phoneNumberDTO)
        {
            try
            {

                Site existingSite = await _db.Sites.FirstOrDefaultAsync(s => s.Id == phoneNumberDTO.Id);
                phoneNumberDTO.CreatedTime = existingSite.CreatedTime;
                phoneNumberDTO.UpdatedTime = DateTime.Now; 
                PhoneNumber phoneNumber = _mapper.Map<PhoneNumber>(phoneNumberDTO);
                _db.PhoneNumbers.Update(phoneNumber);
                _db.SaveChanges();
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
                _response.Code = 200;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public ResponseDTO Delete(int Id)
        {
            try
            {
                PhoneNumber phoneNumber = _db.PhoneNumbers.First(u => u.Id == Id);
                _db.PhoneNumbers.Remove(phoneNumber);
                _db.SaveChanges();
                _response.Code = 200;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}

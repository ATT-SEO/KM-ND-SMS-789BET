using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/[controller]")]
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
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<PhoneNumber> phoneNumbers = _db.PhoneNumbers.Include(u => u.Site).ToList();
                _response.Result = _mapper.Map<IEnumerable<PhoneNumber>>(phoneNumbers);
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
        public ResponseDTO Get(int id)
        {
            try
            {
                PhoneNumber phoneNumber = _db.PhoneNumbers.Include(u => u.Site).First(s => s.Id == id);
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
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
        public ResponseDTO GetByNumber(string Number)
        {
            try
            {
                PhoneNumber phoneNumber = _db.PhoneNumbers.Include(u => u.Site).Where(s => s.Status == true).FirstOrDefault(s => s.Number == Number);
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByNumberSiteID/{Number}/{SiteID}")]
        public ResponseDTO GetByNumberSiteID(string Number, int SiteID)
        {
            try
            {
                PhoneNumber phoneNumber = _db.PhoneNumbers.Include(u => u.Site).Where(s => s.SiteID == SiteID).FirstOrDefault(s => s.Number == Number);
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
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
        public ResponseDTO GetListPhoneBySiteID(int SiteID)
        {
            try
            {
                IEnumerable<PhoneNumber> phoneNumbers = _db.PhoneNumbers.Include(u => u.Site).Where(u => u.SiteID == SiteID).ToList();
                _response.Result = _mapper.Map<IEnumerable<PhoneNumber>>(phoneNumbers);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetlistPhoneBySiteName/{Name}")]
        public ResponseDTO GetlistPhoneBySiteName(string Name)
        {
            try
            {
                IEnumerable<PhoneNumber> phoneNumbers;
                if (!string.IsNullOrEmpty(Name))
                {
                    phoneNumbers = _db.PhoneNumbers
                        .Include(u => u.Site)
                        .Where(s => s.Site != null && s.Site.Name == Name)
                        .ToList();
                    _response.Result = _mapper.Map<IEnumerable<PhoneNumber>>(phoneNumbers);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Giá trị truyền vào lỗi";
                }
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
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] PhoneNumber phoneNumberDTO)
        {
            try
            {
                phoneNumberDTO.UpdatedTime = DateTime.Now;
                PhoneNumber phoneNumber = _mapper.Map<PhoneNumber>(phoneNumberDTO);
                _db.PhoneNumbers.Update(phoneNumber);
                _db.SaveChanges();
                _response.Result = _mapper.Map<PhoneNumber>(phoneNumber);
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

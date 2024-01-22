﻿using API.KM58.Data;
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
                List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Include(u => u.Site).Where(u => u.SiteID == SiteID && u.Status==true).ToListAsync();
                _response.Result = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetListPhoneByProjectID/{ProjectID}")]
        public async Task<ResponseDTO> GetListPhoneByProjectID(string ProjectID)
        {
            try
            {
                Console.WriteLine(ProjectID + "*****************");
                List<PhoneNumber> phoneNumbers = await _db.PhoneNumbers.Include(u => u.Site).Where(u => u.Site.Project == ProjectID).ToListAsync();
                _response.Result = _mapper.Map<List<PhoneNumberDTO>>(phoneNumbers);
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

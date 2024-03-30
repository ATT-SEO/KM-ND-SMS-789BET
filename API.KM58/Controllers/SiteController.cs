using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/settingSite")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        public SiteController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }
        // GET: api/<SiteController>
        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Site> sites = _db.Sites.ToList();
                _response.Result = _mapper.Map<IEnumerable<Site>>(sites);
                _response.Code = 200;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
        // GET api/<SiteController>/5
        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Get(int Id)
        {
            try
            {
                Site site = _db.Sites.First(u => u.Id == Id);
                _response.Result = _mapper.Map<Site>(site);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByProjectCode/{ProjectCode}")]
        public async Task<ResponseDTO> GetByProjectCode(string ProjectCode)
        {
            try
            {
                Site site = await _db.Sites.Where(u => u.Project == ProjectCode).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<Site>(site);
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

        // POST api/<SiteController>
        [HttpPost]
        public ResponseDTO Post([FromBody] Site siteDTO)
        {
            try
            {
                siteDTO.CreatedTime = DateTime.Now;
                siteDTO.UpdatedTime = DateTime.Now;
                Site site = _mapper.Map<Site>(siteDTO);
                _db.Sites.Add(site);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Site>(site);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        //public ResponseDTO Put([FromBody] Site siteDTO)
        //{
        //    try
        //    {
        //        siteDTO.UpdatedTime = DateTime.Now;
        //        Site site = _mapper.Map<Site>(siteDTO);
        //        _db.Sites.Update(site);
        //        _db.SaveChanges();
        //        _response.Result = _mapper.Map<Site>(site);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return _response;
        //}
        public async Task<ResponseDTO> Put([FromBody] JObject siteDTO)
        {
            try
            {
                Log.Information($"UPDATE DATA SITE  || DATA : {JsonConvert.SerializeObject(siteDTO)} ");
                //DateTimeOffset timeOffset = new DateTimeOffset(DateTime.UtcNow);
                //long timeStamp = timeOffset.ToUnixTimeMilliseconds();
                //Console.WriteLine(timeStamp);
                int siteId = (int)siteDTO["Id"];
                Site existingSite = await _db.Sites.FirstOrDefaultAsync(s => s.Id == siteId);
                if (existingSite != null)
                {
                    Log.Information($"UPDATE SITE  || ID : {existingSite.Id} ");
                    foreach (var property in siteDTO)
                    {
                        var siteProperty = existingSite.GetType().GetProperty(property.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (siteProperty != null && siteProperty.CanWrite)
                        {
                            var value = Convert.ChangeType(property.Value, siteProperty.PropertyType);
                            siteProperty.SetValue(existingSite, value);
                        }
                    }

                    existingSite.UpdatedTime = DateTime.Now;

                    _db.Sites.Update(existingSite);
                    _db.SaveChanges();
                    _response.IsSuccess = true;
                    _response.Code = 200;
                    _response.Message = "Cập nhật thành công.";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Cập nhật thất bại.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message; // Xử lý lỗi nếu có
                _response.Code = 500;
            }
            return _response;
        }


        [HttpDelete]
        [Route("Delete/{Id}")]
        public ResponseDTO Delete(int Id)
        {
            try
            {
                Site obj = _db.Sites.First(u => u.Id == Id);
                _db.Sites.Remove(obj);
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

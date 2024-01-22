using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/[controller]")]
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
        [Route("GetByProjectID/{ProjectID}")]
        public async Task<ResponseDTO> GetByProjectID(string ProjectID)
        {
            try
            {
                List<Site> site = await _db.Sites.Where(u => u.Project == ProjectID).ToListAsync();
                _response.Result = _mapper.Map<List<Site>>(site);
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
        public ResponseDTO Put([FromBody] Site siteDTO)
        {
            try
            {
                siteDTO.UpdatedTime = DateTime.Now;
                Site site = _mapper.Map<Site>(siteDTO);
                _db.Sites.Update(site);
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

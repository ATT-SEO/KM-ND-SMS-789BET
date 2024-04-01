using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/Agent")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        public AgentController(AppDbContext db, IMapper mapper)
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
                List<Agent> agents = await _db.Agents.Include(u => u.Site).ToListAsync();
                _response.Result = _mapper.Map<List<AgentDTO>>(agents);
                _response.Code = 200;
                _response.TotalCount = agents.Count;
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
                Agent agent = await _db.Agents.Include(u => u.Site).Where(s => s.Id == id).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<AgentDTO>(agent);
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
        [Route("GetByName/{Name}")]
        public async Task<ResponseDTO> GetByName(string Name)
        {
            try
            {
                Agent agent = await _db.Agents.AsNoTracking().Include(u => u.Site).Where(s => s.Status == true && s.Name == Name).FirstOrDefaultAsync();
                _response.Result = _mapper.Map<AgentDTO>(agent);
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
        [Route("GetListAgentBySiteID/{SiteID}")]
        public async Task<ResponseDTO> GetListAgentBySiteID(int SiteID)
        {
            try
            {
                List<Agent> agents = await _db.Agents.Include(u => u.Site).Where(u => u.SiteID == SiteID).ToListAsync();
                _response.Result = _mapper.Map<List<AgentDTO>>(agents);
                _response.Code = 200;
                _response.TotalCount = agents.Count;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetListAgentByProjectCode/{ProjectCode}")]
        public async Task<ResponseDTO> GetListAgentByProjectCode(string ProjectCode)
        {
            try
            {
                Console.WriteLine(ProjectCode + "*****************");
                List<Agent> agents = await _db.Agents.Include(u => u.Site).Where(u => u.Site.Project == ProjectCode).ToListAsync();
                _response.Result = _mapper.Map<List<AgentDTO>>(agents);
                _response.Code = 200;
                _response.TotalCount = agents.Count;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] Agent agentDTO)
        {
            try
            {
				Log.Information($"CREATE DATA AGENT  || DATA : {JsonConvert.SerializeObject(agentDTO)} ");
				agentDTO.CreatedTime = DateTime.Now;
                agentDTO.UpdatedTime = DateTime.Now;
                Agent agent = _mapper.Map<Agent>(agentDTO);
                _db.Agents.Add(agentDTO);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Agent>(agent);
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
        public async Task<ResponseDTO> Put([FromBody] Agent agentDTO)
        {
            try
            {
				Log.Information($"UPDATE DATA AGENT  || DATA : {JsonConvert.SerializeObject(agentDTO)} ");

				Agent existingSite = await _db.Agents.AsNoTracking().Where(s => s.Id == agentDTO.Id).FirstOrDefaultAsync();
                agentDTO.CreatedTime = existingSite.CreatedTime;
                agentDTO.UpdatedTime = DateTime.Now; 
                Agent agent = _mapper.Map<Agent>(agentDTO);
                _db.Agents.Update(agent);
                _db.SaveChanges();
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
				Log.Information($"CREATE DATA AGENT  || DATA : {DateTime.Now} ");

				Agent agent = _db.Agents.First(u => u.Id == Id);
                _db.Agents.Remove(agent);
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

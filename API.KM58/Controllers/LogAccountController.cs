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
    [Route("api/LogAccount")]
    [ApiController]
    public class LogAccountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        public LogAccountController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }
        // GET: api/<LogAccountController>
        [HttpGet]
        public async Task<ResponseDTO> Get([FromQuery] QueryParametersDTO parameters)
        {
            try
            {
                var query = _db.LogAccounts.AsQueryable();
                if (!string.IsNullOrEmpty(parameters.IP))
                {
                    query = query.Where(w => w.IP == parameters.IP);
                }
                if (!string.IsNullOrEmpty(parameters.FP))
                {
                    query = query.Where(w => w.FP == parameters.FP);
                }
                if (!string.IsNullOrEmpty(parameters.Account))
                {
                    query = query.Where(w => w.Account == parameters.Account);
                }
                int skipCount = (parameters.Page - 1) * parameters.PageSize;
                IEnumerable<LogAccount> LogAccountList = await query
                    .OrderByDescending(w => w.Id)
                    .Skip(skipCount)
                    .Take(parameters.PageSize)
                    .ToListAsync();
                int totalCount = query.Count();
                var result = new
                {
                    Data = _mapper.Map<IEnumerable<LogAccount>>(LogAccountList),
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


        [HttpGet]
        [Route("GetLogAccountListByProjectCode")]
        public async Task<ResponseDTO> GetLogAccountListByProjectCode([FromQuery] QueryParametersDTO parameters)
        {
            try
            {
                var query = _db.LogAccounts.AsQueryable();
                if (!string.IsNullOrEmpty(parameters.ProjectCode))
                {
                    query = query.Where(w => w.Project == parameters.ProjectCode);
                }
                if (!string.IsNullOrEmpty(parameters.IP))
                {
                    query = query.Where(w => w.IP == parameters.IP);
                }
                if (!string.IsNullOrEmpty(parameters.FP))
                {
                    query = query.Where(w => w.FP == parameters.FP);
                }
                if (!string.IsNullOrEmpty(parameters.Account))
                {
                    query = query.Where(w => w.Account == parameters.Account);
                }
                int skipCount = (parameters.Page - 1) * parameters.PageSize;
                List<LogAccount> LogAccountList = await query
                    .OrderByDescending(w => w.Id)
                    .Skip(skipCount)
                    .Take(parameters.PageSize)
                    .ToListAsync();
                int totalCount = query.Count();
                var result = new
                {
                    Data = _mapper.Map<List<LogAccount>>(LogAccountList),
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

        // GET api/<LogAccountController>/5
        [HttpGet]
        [Route("{Id}")]
        public ResponseDTO Get(int Id)
        {
            try
            {
                LogAccount logAccount = _db.LogAccounts.First(u => u.Id == Id);
                _response.Result = _mapper.Map<LogAccount>(logAccount);
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetListByAccount/{Account}")]
        public async Task<ResponseDTO> GetListByAccount(string Account, int page = 1, int pageSize = 100)
        {
            try
            {
                int skipCount = (page - 1) * pageSize;
                IEnumerable<LogAccount> logAccounts = await _db.LogAccounts
                .Where(w => w.Account == Account)
                .OrderByDescending(w => w.Id)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
                int totalCount = _db.LogAccounts.Where(w => w.Account == Account).Count();
                var result = new
                {
                    Data = _mapper.Map<IEnumerable<LogAccount>>(logAccounts),
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

        // POST api/<LogAccountController>
        [HttpPost]
        public async Task<ResponseDTO> Post(LogAccount logAccountDTO)
        {
            try
            {
                LogAccount logAccount1 = await _db.LogAccounts.FirstOrDefaultAsync(s => (s.IP == logAccountDTO.IP || s.FP == logAccountDTO.FP) && s.Project == logAccountDTO.Project);

                if (logAccount1 == null)
                {
                    logAccountDTO.CreatedTime = DateTime.Now;
                    LogAccount logAccount = _mapper.Map<LogAccount>(logAccountDTO);
                    _db.LogAccounts.Add(logAccountDTO);
                    _db.SaveChanges();
                    _response.Result = _mapper.Map<LogAccount>(logAccount);
                }
                else
                {
                    if (logAccount1.Account == logAccountDTO.Account)
                    {
                        _response.Result = _mapper.Map<LogAccount>(logAccount1);
                        _response.IsSuccess = true;
                        _response.Message = "Đã có SMS ở hệ thống.";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Dấu hiệu bất thường.";
                    }
                }

            }
            catch (Exception Ex)
            {

                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
        // DELETE api/<LogAccountController>/5
        [HttpDelete("DeleteLog/{Id}")]
        public ResponseDTO Delete(int Id)
        {
            try
            {
                LogAccount obj = _db.LogAccounts.First(u => u.Id == Id);
                _db.LogAccounts.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
    }
}

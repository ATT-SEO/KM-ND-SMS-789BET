using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRegistersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public AccountRegistersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }
        // GET: api/<AccountRegistersController>
        [HttpGet]
        public async Task<ResponseDTO?> Get([FromQuery] QueryParametersDTO parameters)
        {
            try
            {
                var query = _db.AccountRegisters.AsQueryable();
                if (!string.IsNullOrEmpty(parameters.ProjectCode))
                {
                    query = query.Where(w => w.ProjectCode == parameters.ProjectCode);
                }
                if (!string.IsNullOrEmpty(parameters.Account))
                {
                    query = query.Where(w => w.Account == parameters.Account);
                }
                if (!string.IsNullOrEmpty(parameters.Sender))
                {
                    query = query.Where(w => w.Sender == parameters.Sender);
                }
                if (!string.IsNullOrEmpty(parameters.Content))
                {
                    query = query.Where(w => w.Content == parameters.Content);
                }
                if (parameters.Status.HasValue)
                {
                    query = query.Where(w => w.Status == (int)parameters.Status);
                }
                if (!string.IsNullOrEmpty(parameters.ProjectCode))
                {
                    query = query.Where(w => w.ProjectCode == parameters.ProjectCode);
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
                IEnumerable<AccountRegisters> accountRegistersList = await query
                    .Skip(skipCount)
                    .Take(parameters.PageSize)
                    .ToListAsync();

                int totalCount = query.Count();
                _response.TotalCount = totalCount;
                _response.Result = _mapper.Map<IEnumerable<AccountRegistersDTO>>(accountRegistersList);
                _response.Code = 200;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
                _response.Code = 500;
            }

            return _response;
        }

        // GET api/<AccountRegistersController>/5
        [HttpGet]
        [Route("GetByID/{Id}")]
        public async Task<ResponseDTO> GetByID(int Id)
        {

            AccountRegisters accountRegister = await _db.AccountRegisters.AsNoTracking().Where(x => x.Id == Id).FirstAsync();
            if (accountRegister != null)
            {
                _response.TotalCount = 1;
                _response.Result = _mapper.Map<AccountRegisters>(accountRegister);
                _response.Code = 200;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Account Register  NOT FOUND";
                _response.Code = 500;
                
            }
            return _response;
        }

        // POST api/<AccountRegistersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{

        //}

        // PUT api/<AccountRegistersController>/5
        [HttpPut]
        public ResponseDTO Put([FromBody] AccountRegisters accountRegistersDTO)
        {
            try
            {
                accountRegistersDTO.UpdatedTime = DateTime.Now;
                AccountRegisters accountRegisters = _mapper.Map<AccountRegisters>(accountRegistersDTO);
                _db.AccountRegisters.Update(accountRegisters);
                _db.SaveChanges();
                _response.Result = _mapper.Map<AccountRegisters>(accountRegisters);
                _response.Code=200;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Route("UpdateStatus")]
        public async Task<ResponseDTO> UpdateStatus([FromBody] AccountRegisters accountRegistersDTO)
        {
            try
            {
                var accountToUpdate = await _db.AccountRegisters.Where(a => a.Id == accountRegistersDTO.Id && a.Account == accountRegistersDTO.Account).FirstOrDefaultAsync();
                if (accountToUpdate != null)
                {
                    accountToUpdate.Status = (int)accountRegistersDTO.Status;
                    _db.SaveChanges();
                    _response.IsSuccess = true;
                    _response.Code = 200;
                    if((int)accountRegistersDTO.Status == 9)
                    {
                        _response.Message = $"Tài khoản {accountRegistersDTO.Account} đã bị từ chối nhận thưởng khuyến mãi.";
                        Log.Information($"TAI KHOAN || {accountRegistersDTO.Account} || {accountRegistersDTO.ProjectCode} || TỪ CHỐI NHẬN THƯỞNG");

                    }
                    if ((int)accountRegistersDTO.Status == 1)
                    {
                        _response.Message = $"Tài khoản {accountRegistersDTO.Account} đã được cộng điểm khuyến mãi.";
                        Log.Information($"TAI KHOAN || {accountRegistersDTO.Account} || {accountRegistersDTO.Point} ĐIỂM || CỘNG DIỂM THÀNH CÔNG");
                    }
                }
                else
                {
                    Log.Information($"ERROR TAI KHOAN || {accountRegistersDTO.Account} || {accountRegistersDTO.Point} ĐIỂM || CỘNG DIỂM THẤT BẠI");
                    _response.IsSuccess = false;
                    _response.Code = 500;
                    _response.Message = "Account not found.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message; // Xử lý lỗi nếu có
            }
            return _response;
        }
        // DELETE api/<AccountRegistersController>/5
        [HttpDelete]
        [Route("Id")]
        public async Task<ResponseDTO> Delete(int Id)
        {
            try
            {
                AccountRegisters obj = _db.AccountRegisters.First(u => u.Id == Id);
                _db.AccountRegisters.Remove(obj);
                _db.SaveChanges();
                _response.Message = "Yêu cầu nhận thưởng đã được xóa";
                _response.Code = 200;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
                _response.Code = 500;
            }
            return _response;
        }
    }
}

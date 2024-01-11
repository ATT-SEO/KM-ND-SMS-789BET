using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
	[Route("api/[controller]")]
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
		public ResponseDTO Get(int page = 1, int pageSize = 100)
		{
			try
			{
				int skipCount = (page - 1) * pageSize;
				IEnumerable<LogAccount> LogAccountList = _db.LogAccounts
					.OrderByDescending(w => w.Id)
					.Skip(skipCount)
					.Take(pageSize)
					.ToList();
				int totalCount = _db.LogAccounts.Count();
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
		public ResponseDTO GetListByAccount(string Account, int page = 1, int pageSize = 100)
		{
			try
			{
				int skipCount = (page - 1) * pageSize;
				IEnumerable<LogAccount> logAccounts = _db.LogAccounts
				.Where(w => w.Account == Account)
				.OrderByDescending(w => w.Id)
				.Skip(skipCount)
				.Take(pageSize)
				.ToList();
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
		public ResponseDTO Post(LogAccount logAccountDTO)
		{
			try
			{
                logAccountDTO.CreatedTime = DateTime.Now;
                LogAccount logAccount = _mapper.Map<LogAccount>(logAccountDTO);
				_db.LogAccounts.Add(logAccountDTO);
				_db.SaveChanges();
				_response.Result = _mapper.Map<LogAccount>(logAccount);

			}
			catch (Exception Ex)
			{

				_response.IsSuccess = false;
				_response.Message = Ex.Message;
			}
			return _response;
		}
		// DELETE api/<LogAccountController>/5
		[HttpDelete("DeleteLog/{id}")]
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

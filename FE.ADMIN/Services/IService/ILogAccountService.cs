using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
	public interface ILogAccountService
	{
		Task<ResponseDTO> GetAllLogAccountAsync(int page = 1, int pageSize = 100);
		Task<ResponseDTO> GetLogAccountByIdAsync(int Id);
		Task<ResponseDTO> GetListLogAccountBySiteIDAsync(int? SettingID, int page = 1, int pageSize = 100);
		Task<ResponseDTO> CreateAsync(LogAccountDTO logAccountDTO);
		Task<ResponseDTO> EditAsync(LogAccountDTO logAccountDTO);
		Task<ResponseDTO> DeleteAsync(int id);
	}
}

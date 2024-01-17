using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface ILogAccountService
	{
        Task<ResponseDTO> GetAllLogAccountAsync(QueryParametersDTO parameters);
        Task<ResponseDTO> GetLogAccountByIdAsync(int Id);
        Task<ResponseDTO> GetListLogAccountBySiteIDAsync(int? SiteID, QueryParametersDTO parameters);
        Task<ResponseDTO> CreateAsync(LogAccountDTO logAccountDTO);
		Task<ResponseDTO> EditAsync(LogAccountDTO logAccountDTO);
		Task<ResponseDTO> DeleteAsync(int id);
	}
}

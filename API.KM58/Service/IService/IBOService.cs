
using API.KM58.Model;
using API.KM58.Model.DTO;

namespace API.KM58.Service.IService
{
	public interface IBOService
	{
        Task<ResponseDTO?> BOGetCheckAccount(string Account);
        Task<ResponseDTO?> BOGetCheckAccountCMD(string Account);
        Task<ResponseDTO?> addPointClient(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks);
        Task<ResponseDTO?> addPointBo789BET(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks);
        Task<ResponseDTO?> handApproveAccount(AccountRegistersDTO accountRegisters, Site siteDTO);
        Task<ResponseDTO?> savePointBoAuto789BET(AccountRegistersDTO accountRegisters, Site siteDTO, bool status = true);
        Task<ResponseDTO?> addPointClientCMD(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks);
    }
}

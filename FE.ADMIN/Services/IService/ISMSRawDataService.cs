using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface ISMSRawDataService
    {
		Task<ResponseDTO?> GetAllAsync(QueryParametersDTO parameters);
		//Task<ResponseDTO?> GetByStatus(int Status);
		//Task<ResponseDTO?> GetDateTimeEndDevice(string Device);
		//Task<ResponseDTO?> GetTotalSMS(int Total, string Device);
		//Task<ResponseDTO?> GetOneSMSByID(int Id);
		//Task<ResponseDTO?> EditAsync(SMSDTO sMSDTO);
		//Task<ResponseDTO?> DeleteAsync(int id);
	}
}

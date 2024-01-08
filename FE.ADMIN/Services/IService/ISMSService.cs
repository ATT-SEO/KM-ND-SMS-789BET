using FE.ADMIN.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.ADMIN.Services.IService
{
    public interface ISMSService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByStatus(int Status);
        Task<ResponseDTO?> GetDateTimeEndDevice(string Device);
        Task<ResponseDTO?> GetTotalSMS(int Total, string Device);
        Task<ResponseDTO?> GetOneSMSByID(int Id);
        Task<ResponseDTO?> EditAsync(SMSDTO sMSDTO);
        Task<ResponseDTO?> DeleteAsync(int id);
    }
}

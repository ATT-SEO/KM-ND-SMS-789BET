using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface IPhoneNumberService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByIDAsync(int ID);
        Task<ResponseDTO?> GetByNumberAsync(string Number);
        Task<ResponseDTO?> GetListPhoneBySiteIDAsync(int SiteID);
        Task<ResponseDTO?> GetlistPhoneBySiteName(string SiteName);
    }
}

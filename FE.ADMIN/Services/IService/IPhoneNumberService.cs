using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface IPhoneNumberService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByIDAsync(int ID);
        Task<ResponseDTO?> GetByNumberAsync(string Number);
        Task<ResponseDTO?> GetListPhoneBySiteIDAsync(int SiteID);
        Task<ResponseDTO?> GetlistPhoneBySiteName(string SiteName);
        Task<ResponseDTO?> CreateAsync(PhoneNumberDTO phoneNumberDTO);
        Task<ResponseDTO?> EditAsync(PhoneNumberDTO phoneNumberDTO);
        Task<ResponseDTO?> DeleteAsync(int id);
    }
}

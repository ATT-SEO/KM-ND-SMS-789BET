using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface IPhoneNumberService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByIDAsync(int ID);
        Task<ResponseDTO?> GetByNumberAsync(string Number);
        Task<ResponseDTO?> GetListPhoneBySiteIDAsync(int SiteID);
        Task<ResponseDTO?> GetListPhoneByProjectCode(string SiteName);
        Task<ResponseDTO?> Post(PhoneNumberDTO phoneNumberDTO);
        Task<ResponseDTO?> Delete(int id);
        Task<ResponseDTO?> Edit(PhoneNumberDTO phoneNumberDTO);
    }
}

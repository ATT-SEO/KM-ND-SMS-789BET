using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface IPromotionClaimService
    {
        Task<ResponseDTO?> GetAllPromotionClaimAsync();
        Task<ResponseDTO?> DeleteAsync(int id);
        //Task<ResponseDTO?> GetTargetBySiteIdAsync(int SettingID);
        //Task<ResponseDTO?> GetTargetByIdAsync(int id);
        //Task<ResponseDTO?> CreateAsync(TargetListDTO myTargetSiteDTO);
        //Task<ResponseDTO?> EditAsync(TargetListDTO myTargetSiteDTO);
        //Task<ResponseDTO?> DeleteAsync(int id);
    }
}

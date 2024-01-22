using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface ISiteService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetSiteBytitleAsync(string Name);
       // Task<ResponseDTO?> GetUserSiteBySiteAsync(string Site, string userId);
        Task<ResponseDTO?> GetSiteByIDAsync(int ID);
        Task<ResponseDTO?> CreateAsync(SiteDTO siteDTO);
        Task<ResponseDTO?> EditAsync(SiteDTO siteDTO);
        Task<ResponseDTO?> DeleteAsync(int id);
        Task<ResponseDTO?> GetByProjectID(String ProjectID);
    }
}

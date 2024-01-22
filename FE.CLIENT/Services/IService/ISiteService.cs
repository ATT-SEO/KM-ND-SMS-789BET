using FE.CLIENT.Models;

namespace FE.CLIENT.Services.IService
{
    public interface ISiteService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetSiteBytitleAsync(string Name);
       // Task<ResponseDTO?> GetUserSiteBySiteAsync(string Site, string userId);
        Task<ResponseDTO?> GetSiteByIDAsync(int ID);
        Task<ResponseDTO?> GetByProjectCode(string ProjectCode);
    }
}

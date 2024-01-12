using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface ISiteService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetSiteBytitleAsync(string Name);
        // Task<ResponseDTO?> GetUserSiteBySiteAsync(string Site, string userId);
        Task<ResponseDTO?> GetSiteByIDAsync(int ID);
    }
}

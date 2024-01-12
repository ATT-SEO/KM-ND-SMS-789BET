
using FE.JunCMD.Client.Models;
using FE.JunCMD.Client.Services.IService;
using FE.JunCMD.Client.Utility;

namespace FE.JunCMD.Client.Services
{
    public class SiteService : ISiteService
    {
        private readonly IBaseService _baseService;
        public SiteService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/Site"
            });
        }

        public async Task<ResponseDTO?> GetSiteByIDAsync(int ID)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/Site/" + ID
            });
        }

        public async Task<ResponseDTO?> GetSiteBytitleAsync(string Name)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/Site/GetBySite/" + Name
            });
        }
    }
}

using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using FE.CLIENT.Utility;

namespace FE.CLIENT.Services
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
                Url = SD.ApiKM58 + "/api/settingSite/" + ID
            });
        }

        public async Task<ResponseDTO?> GetSiteBytitleAsync(string Name)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/settingSite/GetBySite/" + Name
            });
        }

        public async Task<ResponseDTO?> GetByProjectCode(string ProjectCode)
        { 
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/settingSite/GetByProjectCode/" + ProjectCode
            });
        }
    }
}

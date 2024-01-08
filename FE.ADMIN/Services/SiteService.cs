using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;

namespace FE.ADMIN.Services
{
    public class SiteService : ISiteService
    {
        private readonly IBaseService _baseService;
        public SiteService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateAsync(SiteDTO siteDTO)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = siteDTO,
                Url = SD.ApiKM58 + "/api/Site"
            });
        }

        public async Task<ResponseDTO?> DeleteAsync(int id)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.DELETE,
                Url = SD.ApiKM58 + "/api/Site/Delete/" + id
            });
        }

        public async Task<ResponseDTO?> EditAsync(SiteDTO siteDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.PUT,
                Data = siteDTO,
                Url = SD.ApiKM58 + "/api/Site"
            });

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

using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using FE.CLIENT.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FE.CLIENT.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IBaseService _baseService;
        public PhoneNumberService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber"
            });
        }

        public async Task<ResponseDTO?> GetByIDAsync(int ID)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/" + ID
            });
        }

        public async Task<ResponseDTO?> GetByNumberAsync(string Number)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetByNumber/" + Number
            });
        }

        public async Task<ResponseDTO?> GetListPhoneBySiteIDAsync(int SiteID)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetListPhoneBySiteID/" + SiteID
            });
        }

        public async Task<ResponseDTO?> GetlistPhoneBySiteName(string SiteName)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetlistPhoneBySiteName/" + SiteName
            });
        }
    }
}

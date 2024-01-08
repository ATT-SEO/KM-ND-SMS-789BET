using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using FE.CLIENT.Utility;

namespace FE.CLIENT.Services
{
    public class SMSService : ISMSService
    {
        private readonly IBaseService _baseService;

        public SMSService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CheckSMSWebsite(SMSDTO smsDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = smsDTO,
                Url = SD.ApiKM58 + "/api/SMS/CheckSMSWebsite"
            });
        }

        public async Task<ResponseDTO?> CreateWebsiteAsync(SMSDTO smsDTO)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = smsDTO,
                Url = SD.ApiKM58 + "/api/SMS/PostWebsite"
            });
        }
    }
}

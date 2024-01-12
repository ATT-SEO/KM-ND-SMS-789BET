
using FE.JunCMD.Client.Models;
using FE.JunCMD.Client.Services.IService;
using FE.JunCMD.Client.Utility;

namespace FE.JunCMD.Client.Services
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

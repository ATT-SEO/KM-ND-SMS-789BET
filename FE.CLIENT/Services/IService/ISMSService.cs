using FE.CLIENT.Models;

namespace FE.CLIENT.Services.IService
{
    public interface ISMSService
    {
        Task<ResponseDTO?> CreateWebsiteAsync(SMSDTO smsDTO);
        Task<ResponseDTO?> CheckSMSWebsite(SMSDTO smsDTO);
    }
}

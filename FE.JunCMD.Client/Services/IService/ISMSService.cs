using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface ISMSService
    {
        Task<ResponseDTO?> CreateWebsiteAsync(SMSDTO smsDTO);
        Task<ResponseDTO?> CheckSMSWebsite(SMSDTO smsDTO);
    }
}

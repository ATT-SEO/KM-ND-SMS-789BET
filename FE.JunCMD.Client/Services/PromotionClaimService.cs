
using FE.JunCMD.Client.Models;
using FE.JunCMD.Client.Services.IService;
using FE.JunCMD.Client.Utility;

namespace FE.JunCMD.Client.Services
{
    public class PromotionClaimService : IPromotionClaimService
    {
        private readonly IBaseService _baseService;
        public PromotionClaimService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllPromotionClaimAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PromotionClaim"
            });
        }

        public async Task<ResponseDTO?> DeleteAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.DELETE,
                Url = SD.ApiKM58 + "/api/PromotionClaim/Delete/" + id
            });
        }

    }
}

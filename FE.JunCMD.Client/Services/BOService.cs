using FE.JunCMD.Client.Models;
using FE.JunCMD.Client.Services.IService;
using FE.JunCMD.Client.Utility;
using System;

namespace FE.JunCMD.Client.Services
{
    public class BOService : IBOService
    {
        private readonly IBaseService _baseService;
        public BOService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> BoCheckUserAccount(string UserAccount)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/BOAPI/CheckAccountCMDUserName/" + UserAccount
            });
        }
    }
}

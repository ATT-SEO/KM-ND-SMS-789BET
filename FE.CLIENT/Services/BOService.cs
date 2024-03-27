using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using FE.CLIENT.Utility;
using System;

namespace FE.CLIENT.Services
{
    public class BOService : IBOService
    {
        private readonly IBaseService _baseService;
        public BOService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> BoCheckUserAccount(CheckAccountRequestDTO accountRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = accountRequestDTO,
                Url = SD.ApiKM58 + "/api/BOAPI/CheckAccountUserName/"
            });
        }
    }
}

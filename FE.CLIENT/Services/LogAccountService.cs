using FE.CLIENT.Models;
using FE.CLIENT.Services.IService;
using FE.CLIENT.Utility;

namespace FE.CLIENT.Services
{
    public class LogAccountService : ILogAccountService
    {
        private readonly IBaseService _baseService;
        public LogAccountService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateAsync(LogAccountDTO logAccountDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = logAccountDTO,
                Url = SD.ApiKM58 + "/api/LogAccount"
            });
        }
    }
}

using FE.ADMIN.Services.IService;
using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Newtonsoft.Json;

namespace FE.ADMIN.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.AuthAPIBase + "/api/Account/assignrole",
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            String jsonString = JsonConvert.SerializeObject(requestDTO);
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.AuthAPIBase + "/api/Account/Login",
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.AuthAPIBase + "/api/Account/register",
            }, withBearer: false);
        }
    }
}

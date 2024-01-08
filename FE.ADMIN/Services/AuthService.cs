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

        public async Task<ResponseDTO?> GetAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/auth",
            });
        }

		public async Task<ResponseDTO?> DeleteAsync(String IdentityUser)
		{
			return await _baseService.SendAsync(new RequestDTO()
			{
				APIType = SD.APIType.DELETE,
				Url = SD.ApiKM58 + "/api/auth?UserID="+ IdentityUser,
			});
		}

		public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.ApiKM58 + "/api/auth/assignrole",
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.ApiKM58 + "/api/auth/Login",
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = requestDTO,
                Url = SD.ApiKM58 + "/api/auth/register",
            }, withBearer: false);
        }

		
	}
}

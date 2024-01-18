using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);
        Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO requestDTO);
        Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO requestDTO);
    }
}

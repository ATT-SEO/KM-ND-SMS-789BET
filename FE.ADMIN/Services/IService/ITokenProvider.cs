using FE.ADMIN.Models;

namespace FE.ADMIN.Services.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
        UserDTO ReadTokenClearInformation();
    }
}

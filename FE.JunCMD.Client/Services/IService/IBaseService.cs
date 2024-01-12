using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBearer = true);
    }
}

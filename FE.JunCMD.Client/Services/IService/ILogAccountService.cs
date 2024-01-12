using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface ILogAccountService
    {
        Task<ResponseDTO> CreateAsync(LogAccountDTO logAccountDTO);

    }
}

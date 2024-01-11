using FE.CLIENT.Models;

namespace FE.CLIENT.Services.IService
{
    public interface ILogAccountService
    {
        Task<ResponseDTO> CreateAsync(LogAccountDTO logAccountDTO);

    }
}

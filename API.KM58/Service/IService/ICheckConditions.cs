using API.KM58.Model;
using API.KM58.Model.DTO;

namespace API.KM58.Service.IService
{
    public interface ICheckConditions
    {
        Task<ResponseDTO?> CheckLogAccount(LogAccountDTO logAccountDTO);
        Task<ResponseDTO?> CheckAccountRegisters(AccountRegistersDTO accountRegistersDTO, Site oneSite);
        Task<ResponseDTO?> CheckAccountSMS(SMSDTO smsDTO);
        Task<ResponseDTO?> CheckOneAccountRegisters(string Account, string Project);
        Task<ResponseDTO?> CheckOneAccountSMS(string Account);

    }
}

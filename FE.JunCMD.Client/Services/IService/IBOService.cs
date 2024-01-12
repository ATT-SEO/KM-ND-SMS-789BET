using FE.JunCMD.Client.Models;

namespace FE.JunCMD.Client.Services.IService
{
    public interface IBOService
    {
        Task<ResponseDTO?> BoCheckUserAccount(string UserAccount);
    }
}

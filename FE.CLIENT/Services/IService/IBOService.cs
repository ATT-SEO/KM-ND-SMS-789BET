using FE.CLIENT.Models;

namespace FE.CLIENT.Services.IService
{
    public interface IBOService
    {
        Task<ResponseDTO?> BoCheckUserAccount(string UserAccount);
    }
}

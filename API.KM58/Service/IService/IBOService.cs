
using API.KM58.Model.DTO;

namespace API.KM58.Service.IService
{
	public interface IBOService
	{
        Task<ResponseDTO?> BOGetCheckAccount(string Account);

        Task<ResponseDTO?> addPointClient(String Site, String Account, int Point, String PromoID, int Round);

    }
}

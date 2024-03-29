using API.KM58.Model.DTO;
using System.Threading.Tasks;

namespace API.KM58.Service.IService
{
	public interface IGoogleSheetService
	{
		Task<ResponseDTO?> WriteToGoogleSheets(string Account, string IP, string FP, string Reason = null, string Error = null);
	}
}

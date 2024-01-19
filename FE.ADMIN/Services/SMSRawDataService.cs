using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;

namespace FE.ADMIN.Services
{
    public class SMSRawDataService : ISMSRawDataService
    {
        private readonly IBaseService _baseService;
        public SMSRawDataService(IBaseService baseService)
        {
            _baseService = baseService;
        }

		public async Task<ResponseDTO?> GetAllAsync(QueryParametersDTO parameters)
		{
			string url = $"{SD.ApiKM58}/api/SMSRawData?{CoreBase.ToQueryString(parameters)}";

			return await _baseService.SendAsync(new RequestDTO()
			{
				APIType = SD.APIType.GET,
				Url = url
			});
		}


		//public Task<ResponseDTO?> DeleteAsync(int id)
		//{
		//    throw new NotImplementedException();
		//}

		//public Task<ResponseDTO?> EditAsync(SMSDTO sMSDTO)
		//{
		//    throw new NotImplementedException();
		//}

		//public async Task<ResponseDTO?> GetAllAsync()
		//{
		//    return await _baseService.SendAsync(new RequestDTO()
		//    {
		//        APIType = SD.APIType.GET,
		//        Url = SD.ApiKM58 + "/api/SMS"
		//    });
		//}

		//public async Task<ResponseDTO?> GetByStatus(int Status)
		//{
		//    return await _baseService.SendAsync(new RequestDTO()
		//    {
		//        APIType = SD.APIType.GET,
		//        Url = SD.ApiKM58 + "/api/SMS/GetByStatus/" + Status
		//    });
		//}

		//public async Task<ResponseDTO?> GetDateTimeEndDevice(string Device)
		//{
		//    return await _baseService.SendAsync(new RequestDTO()
		//    {
		//        APIType = SD.APIType.GET,
		//        Url = SD.ApiKM58 + "/api/SMS/GetDateTimeEnd/" + Device
		//    });
		//}

		//public async Task<ResponseDTO?> GetOneSMSByID(int Id)
		//{
		//    return await _baseService.SendAsync(new RequestDTO()
		//    {
		//        APIType = SD.APIType.GET,
		//        Url = SD.ApiKM58 + "/api/SMS/" + Id
		//    });
		//}

		//public async Task<ResponseDTO?> GetTotalSMS(int Total, string Device)
		//{
		//    return await _baseService.SendAsync(new RequestDTO()
		//    {
		//        APIType = SD.APIType.GET,
		//        Url = SD.ApiKM58 + $"/api/SMS/GetTotalSMS/?Total={Total}&Device={Device}"
		//    });
		//}
	}
}

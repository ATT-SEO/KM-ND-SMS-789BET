using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;

namespace FE.ADMIN.Services
{
    public class SMSService : ISMSService
    {
        private readonly IBaseService _baseService;
        public SMSService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public Task<ResponseDTO?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> EditAsync(SMSDTO sMSDTO)
        {
            throw new NotImplementedException();
        }

        //public async Task<ResponseDTO?> GetAllAsync()
        //{
        //    return await _baseService.SendAsync(new RequestDTO()
        //    {
        //        APIType = SD.APIType.GET,
        //        Url = SD.ApiKM58 + "/api/SMS"
        //    });
        //}
		public async Task<ResponseDTO?> GetAllAsync(QueryParametersDTO parameters)
		{
			string url = $"{SD.ApiKM58}/api/SMS?{ToQueryString(parameters)}";

			return await _baseService.SendAsync(new RequestDTO()
			{
				APIType = SD.APIType.GET,
				Url = url
			});
		}

		public async Task<ResponseDTO?> GetByStatus(int Status)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/SMS/GetByStatus/" + Status
            });
        }

        public async Task<ResponseDTO?> GetDateTimeEndDevice(string Device)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/SMS/GetDateTimeEnd/" + Device
            });
        }

        public async Task<ResponseDTO?> GetOneSMSByID(int Id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/SMS/" + Id
            });
        }
        public async Task<ResponseDTO?> GetTotalSMS(int Total, string Device)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + $"/api/SMS/GetTotalSMS/?Total={Total}&Device={Device}"
            });
        }

		private static string ToQueryString(object obj)
		{
			var properties = obj.GetType().GetProperties();
			var keyValuePairs = properties
				.Where(property => property.GetValue(obj) != null)
				.Select(property => $"{property.Name}={Uri.EscapeDataString(property.GetValue(obj).ToString())}");

			return string.Join("&", keyValuePairs);
		}

        public async  Task<ResponseDTO?> GetAllAccountRegisters(QueryParametersDTO parameters)
        {
            string url = $"{SD.ApiKM58}/api/AccountRegisters?{ToQueryString(parameters)}";

            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = url
            });
        }
    }
}

using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FE.ADMIN.Services
{
    public class LogAccountService : ILogAccountService
    {

        private readonly IBaseService _baseService;

        public LogAccountService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateAsync(LogAccountDTO logAccountDTO)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = logAccountDTO,
                Url = SD.ApiKM58 + "/api/LogAccount"
            });
        }

        public Task<ResponseDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> EditAsync(LogAccountDTO logAccountDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO?> GetAllLogAccountAsync(QueryParametersDTO parameters)
        {
			string url = $"{SD.ApiKM58}/api/LogAccount?{ToQueryString(parameters)}";

			return await _baseService.SendAsync(new RequestDTO()
			{
				APIType = SD.APIType.GET,
				Url = url
			});
		}

        public async Task<ResponseDTO?> GetLogAccountListByProjectID(QueryParametersDTO parameters)
        {
            string url = $"{SD.ApiKM58}/api/LogAccount/GetLogAccountListByProjectID?{ToQueryString(parameters)}";
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = url
            });
        }

        public Task<ResponseDTO> GetListLogAccountBySiteIDAsync(int? SiteID, QueryParametersDTO parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GetLogAccountByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

		private static string ToQueryString(object obj)
		{
			var properties = obj.GetType().GetProperties();
			var keyValuePairs = properties
				.Where(property => property.GetValue(obj) != null)
				.Select(property => $"{property.Name}={Uri.EscapeDataString(property.GetValue(obj).ToString())}");

			return string.Join("&", keyValuePairs);
		}
	}
}

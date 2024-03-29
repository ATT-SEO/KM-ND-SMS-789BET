using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.KM58.Service
{
	public class GoogleSheetService : IGoogleSheetService
	{
		private readonly IHttpClientFactory _httpclientFactory;
		private ResponseDTO _responseDTO;
		public GoogleSheetService(IHttpClientFactory httpClientFactory)
		{
			_httpclientFactory = httpClientFactory;
			_responseDTO = new ResponseDTO();
		}
		public async Task<ResponseDTO?> WriteToGoogleSheets(string Account, string IP , string FP, string Reason = null, string Error = null) 
		{

			try
			{
				HttpClient client = _httpclientFactory.CreateClient();
				HttpRequestMessage message = new HttpRequestMessage();
				message.Method = HttpMethod.Post;
				message.Headers.Add("Accept", "application/json");
				message.RequestUri = new Uri("https://script.google.com/macros/s/AKfycbywazs7U0aZ7oolbb8C2aplu9BjNtp8fCs8CAX7hd_b4HR6XqDxHR-x1y8rjgU9NDU/exec?key=fe5713ac70c5f1f84438edc9349eaf08");
				message.Content = JsonContent.Create(new Dictionary<string, object>
				{
					["account"] = Account,
					["ip"] = IP ?? "",
					["fp"] = FP ?? "",
					["reason"] = Reason ?? "",
					["error"] = Error ?? ""
				});
				var apiResponse = await client.SendAsync(message);

				switch (apiResponse.StatusCode)
				{
					case System.Net.HttpStatusCode.NotFound:
						return new() { IsSuccess = false, Message = "Not Found" };
					case System.Net.HttpStatusCode.Forbidden:
						return new() { IsSuccess = false, Message = "Access Denied" };
					case System.Net.HttpStatusCode.Unauthorized:
						return new() { IsSuccess = false, Message = "Unauthorized" };
					case System.Net.HttpStatusCode.InternalServerError:
						return new() { IsSuccess = false, Message = "Internal Server Error" };
					default:
						_responseDTO.Result = await apiResponse.Content.ReadAsStringAsync();
						return _responseDTO;
				}
			}
			catch (Exception ex)
			{
				var dto = new ResponseDTO
				{
					Message = ex.Message.ToString(),
					IsSuccess = false
				};
				return dto;

			}
		}

	
	}
}

using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using Newtonsoft.Json;
namespace API.KM58.Service
{
    public class BOService : IBOService
    {
        private readonly IHttpClientFactory _httpclientFactory;
        private ResponseDTO _responseDTO;
        public BOService(IHttpClientFactory httpClientFactory)
        {
            _httpclientFactory = httpClientFactory;
            _responseDTO = new ResponseDTO();
        }

        public async Task<ResponseDTO?> addPointClient(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks)
        {
            try
            {
                Console.WriteLine("addPointClient");
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();

                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri($"https://api-bo-jun88k36.khuyenmaiapp.com/api/manualadjusts/add-batch-with-round?site={Site}");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["playerid"] = Account,
                    ["point"] = Point,
                    ["round"] = Round,
                    ["ecremarks"] = Ecremarks,
                    ["remarks"] = Remarks
                });

                var apiResponse = await client.SendAsync(message);
                Console.WriteLine(JsonConvert.SerializeObject(apiResponse));
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
        public async Task<ResponseDTO?> addPointClientCMD(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks)
        {
            try
            {
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri($"https://api-bo-jun88cmd.khuyenmaiapp.com/add-point?site={Site}&username={Account}&point={Point}&round={Round}&note={Ecremarks}");
                var apiResponse = await client.SendAsync(message);
                Console.WriteLine(JsonConvert.SerializeObject(apiResponse));
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

        public async Task<ResponseDTO?> BOGetCheckAccount(string Account)
        {
            try
            {
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri("https://api-bo-jun88k36.khuyenmaiapp.com/api/player/find-player?site=mocbai");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["playerid"] = Account,
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
        public async Task<ResponseDTO?> BOGetCheckAccountCMD(string Account)
        {
            try
            {
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri("https://api-bo-jun88cmd.khuyenmaiapp.com/api/member/get-info-member?site=jun88cmd");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["account"] = Account,
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

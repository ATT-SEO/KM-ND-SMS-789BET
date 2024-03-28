using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Service.IService;
using Newtonsoft.Json;
using Serilog;
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
        public async Task<ResponseDTO?> BOGetCheckAccount(string Account)
        {
            try
            {
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri("https://api-bo-789bet.khuyenmaiapp.com/v1/member/get-data?site=bo_789bet_mk");
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

        public async Task<ResponseDTO?> addPointBo789BET(String Site, String Account, int Point, int Round, String Remarks, String Ecremarks)
        {
            try
            {
                DateTimeOffset timeOffset = new DateTimeOffset(DateTime.UtcNow);
                long timeStamp = timeOffset.ToUnixTimeMilliseconds();

                Console.WriteLine("addPointClient");
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();

                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri($"https://api-bo-789bet.khuyenmaiapp.com/add-point-bo");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["AccountsString"] = Account,
                    ["Amount"] =  1, /// Point
                    ["Audit"] =  1,  /// Round
                    ["Memo"]  = Remarks,  // Nhãn dán dành cho bên mình
                    ["PortalMemo"] = Ecremarks, // hiển thị thông tiêu đề ở người chơi
                    ["TimeStamp"] = timeStamp,
                    ["site"] =  "bo_789bet"
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
                        _responseDTO.Code = 200;
                        _responseDTO.Result = true;
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

        public async Task<ResponseDTO?> handApproveAccount(AccountRegistersDTO accountRegisters, Site siteDTO)
        {
            try
            {
                DateTimeOffset timeOffset = new DateTimeOffset(DateTime.UtcNow);
                long timeStamp = timeOffset.ToUnixTimeMilliseconds();
                Console.WriteLine("addPointClient");
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri($"https://api-bokm.789237.com/registerList/extends");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["site"] = siteDTO.Project,
                    ["accountATT"] = accountRegisters.Account,
                    ["point"] = accountRegisters.Point,
                    ["audit"] = accountRegisters.Audit,
                    ["round"] = siteDTO.Round,
                    ["phone"] = accountRegisters.Sender,
                    ["portal_memo"] = siteDTO.Ecremarks,
                    ["promo_id"] = siteDTO.Remarks,   ///Mã khuyến mãi
                    ["promo_name"] = siteDTO.Label, ///Tên khuyến mãi
                    ["ip"] = accountRegisters.IP,
                    ["fp"] = accountRegisters.FP,
                    ["deviceType"] = "mobile",
                    ["callback_id"] = accountRegisters.Token
                });

                var apiResponse = await client.SendAsync(message);
                Log.Information($"SUCCESS SEVICE BOAPI SEND DATA TTKM || {accountRegisters.Account} || {accountRegisters.ProjectCode}");
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
                        _responseDTO.Code = 200;
                        _responseDTO.Result = true;
                        return _responseDTO;
                }
            }
            catch (Exception ex)
            {
                Log.Information($"ERROR SEVICE BOAPI SEND DATA TTKM || {accountRegisters.Account} || {accountRegisters.ProjectCode} || {ex.Message.ToString()}");
                var dto = new ResponseDTO
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }

        }
        public async Task<ResponseDTO?> savePointBoAuto789BET(AccountRegistersDTO accountRegisters, Site siteDTO, bool status)
        {
            try
            {
                DateTimeOffset timeOffset = new DateTimeOffset(DateTime.UtcNow);
                long timeStamp = timeOffset.ToUnixTimeMilliseconds();

                Console.WriteLine("addPointClient");
                HttpClient client = _httpclientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Post;
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri($"https://api-bokm.789237.com/autoApproveList/extends");
                message.Content = JsonContent.Create(new Dictionary<string, object>
                {
                    ["site"] = siteDTO.Project,
                    ["accountATT"] = accountRegisters.Account,
                    ["point"] = accountRegisters.Point,
                    ["audit"] = accountRegisters.Audit,
                    ["round"] = siteDTO.Round,
                    ["phone"] = accountRegisters.Sender,
                    ["portal_memo"] = siteDTO.Ecremarks,
                    ["promo_id"] = siteDTO.Remarks,
                    ["promo_name"] = siteDTO.Label,
                    ["ip"] = accountRegisters.IP,
                    ["fp"] = accountRegisters.FP,
                    ["deviceType"] = "mobile",
                    ["status"] = status ? "accept" : "error", //  ["accept", "deny", "error"],
                    ["ticket_id"] = null,
                    ["reason_deny"] = "Tự dộng thành công"
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
                        _responseDTO.Code = 200;
                        _responseDTO.Result = true;
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

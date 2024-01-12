using static FE.JunCMD.Client.Utility.SD;

namespace FE.JunCMD.Client.Models
{
    public class RequestDTO
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}

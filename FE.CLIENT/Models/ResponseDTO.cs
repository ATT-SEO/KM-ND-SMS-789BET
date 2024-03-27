namespace FE.CLIENT.Models
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public int TotalCount { get; set; }
        public int Code { get; set; }
    }
}

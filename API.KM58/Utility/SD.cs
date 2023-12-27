namespace API.KM58.Utility
{
    public class SD
    {
        public static string? TelegramAPIBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomner = "CUSTOMER";
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public enum ContentType
        {
            Json,
            MultiPartFormData
        }
    }
}

namespace FE.ADMIN.Utility
{
    public class SD
    {
        public static string? ApiKM58 { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomner = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}

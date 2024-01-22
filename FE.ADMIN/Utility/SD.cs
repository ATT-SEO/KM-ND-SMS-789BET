using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FE.ADMIN.Utility
{
    public class SD
    {
        public static string? ApiKM58 { get; set; }
        public static string? AuthAPIBase { get; set; }

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

        public enum ContentType
        {
            Json,
            MultiPartFormData
        }

        public static string[] Project_Code = { "FREE66", "K58" };

        //public static Dictionary<string, string> ProjectID = new Dictionary<string, string>
        //{
        //    { "MB66_FREE66", "FREE66" },
        //    { "JUN88_CMD_K58", "KM58_Jun88_CMD" }
        //};
    }
}

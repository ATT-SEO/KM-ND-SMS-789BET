using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace FE.ADMIN.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(SD.TokenCookie);
        }

        public string? GetToken()
        {
            string? Token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out Token);
            return hasToken is true ? Token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(SD.TokenCookie, token);
        }

        public UserDTO ReadTokenClearInformation()
        {
            UserDTO myUserResult = new UserDTO();
            string? Token = null;
            bool? hasProjectCode = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out Token);
            if (hasProjectCode == true)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(Token);
                var tokenS = jsonToken as JwtSecurityToken;
                myUserResult.ProjectCode = tokenS.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
                myUserResult.UserName = tokenS.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value;
                myUserResult.Role = tokenS.Claims.First(claim => claim.Type == "role").Value;
            }
            return myUserResult;
        }
    }
}

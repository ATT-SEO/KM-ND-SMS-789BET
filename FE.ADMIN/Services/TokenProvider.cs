﻿using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;

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
    }
}
using Microsoft.AspNetCore.Identity;

namespace API.KM58.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IdentityUser applicationUser, IEnumerable<string> roles);
    }
}

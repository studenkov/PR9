using PR9.Models;

namespace PR9.Services;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
}

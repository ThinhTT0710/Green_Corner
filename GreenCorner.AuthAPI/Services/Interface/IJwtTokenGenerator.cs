using GreenCorner.AuthAPI.Models;

namespace GreenCorner.AuthAPI.Services.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<string> Roles);
    }
}

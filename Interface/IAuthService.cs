

using TelgeProject.Entity;

namespace TelgeProject.Interface
{
    public interface IAuthService
    {
        TblUser Authenticate(string username, string password);
        string GenerateToken(TblUser user);
    }
}

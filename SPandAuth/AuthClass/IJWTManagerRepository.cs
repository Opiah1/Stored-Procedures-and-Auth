using SPandAuth.Data;
using SPandAuth.Models;

namespace SPandAuth.AuthClass
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(UserLogin user);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}

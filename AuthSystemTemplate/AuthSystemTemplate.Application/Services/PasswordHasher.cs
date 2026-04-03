using AuthSystemTemplate.Application.Interfaces.Services;

namespace AuthSystemTemplate.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    public (string hash, string salt) HashPassword(string password)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        throw new NotImplementedException();
    }

    public bool IsPasswordStrong(string password)
    {
        throw new NotImplementedException();
    }
}
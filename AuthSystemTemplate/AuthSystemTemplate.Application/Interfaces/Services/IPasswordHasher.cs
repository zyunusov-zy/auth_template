namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface IPasswordHasher
{
    /// <summary>
    /// Hash password with salt
    /// NFR-1: Password Security (Argon2id)
    /// Returns: "salt:hash" format
    /// </summary>
    (string hash, string salt) HashPassword(string password);
    
    /// <summary>
    /// Verify password against stored hash and salt
    /// </summary>
    bool VerifyPassword(string password, string storedHash, string storedSalt);
    
    /// <summary>
    /// Check if password meets complexity requirements
    /// FR-1.3: Enforce password complexity rules
    /// </summary>
    bool IsPasswordStrong(string password);
}
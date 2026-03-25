namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface IPasswordValidator
{
    /// <summary>
    /// Validate password strength
    /// FR-1.3: Password complexity rules
    /// </summary>
    ValidationResult ValidatePassword(string password);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
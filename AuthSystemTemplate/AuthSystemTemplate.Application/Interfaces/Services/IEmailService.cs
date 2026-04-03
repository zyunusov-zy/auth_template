using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface IEmailService
{
    /// <summary>
    /// Send email verification message
    /// FR-1.5: Send verification email with token link
    /// </summary>
    Task SendEmailVerificationAsync(string toEmail, string verificationToken, string userName);
    
    /// <summary>
    /// Send password reset email
    /// FR-3.3: Send reset email with token link
    /// </summary>
    Task SendPasswordResetEmailAsync(string toEmail, string resetToken, string userName);
    
    /// <summary>
    /// Send password change confirmation
    /// FR-3.7: Send confirmation email after successful reset
    /// </summary>
    Task SendPasswordChangedConfirmationAsync(string toEmail, string userName);
    
    /// <summary>
    /// Send welcome email after successful registration
    /// </summary>
    Task SendWelcomeEmailAsync(User user);
    
    /// <summary>
    /// Generic email sending method
    /// </summary>
    Task SendEmailAsync(string toEmail, string subject, string htmlBody);
}
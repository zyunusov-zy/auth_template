using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Services;

public class EmailService : IEmailService
{
    public Task SendEmailVerificationAsync(string toEmail, string verificationToken, string userName)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetEmailAsync(string toEmail, string resetToken, string userName)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordChangedConfirmationAsync(string toEmail, string userName)
    {
        throw new NotImplementedException();
    }

    public Task SendWelcomeEmailAsync(User user)
    {
        throw new NotImplementedException();
    }
    
    public Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        throw new NotImplementedException();
    }
}
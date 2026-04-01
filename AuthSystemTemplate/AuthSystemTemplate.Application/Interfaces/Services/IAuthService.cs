using AuthSystemTemplate.Application.Common.Results;
using AuthSystemTemplate.Application.DTOs.Auth;

namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface IAuthService
{
    /// <summary>
    /// Register new user
    /// FR-1: User Registration with Email Verification
    /// </summary>
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request);
    
    /// <summary>
    /// Login user and generate tokens
    /// FR-2: Login with JWT Token Generation
    /// </summary>
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    
    /// <summary>
    /// Refresh access token using refresh token
    /// NFR-3: Refresh Token Mechanism
    /// </summary>
    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(string refreshToken);
    
    /// <summary>
    /// Verify email using token
    /// FR-1.6: Token expires after 24 hours
    /// </summary>
    Task<Result<EmailVerificationResponse>> VerifyEmailAsync(string token);
    
    /// <summary>
    /// Resend email verification
    /// FR-1.8: Resend verification email functionality
    /// </summary>
    Task<Result<EmailVerificationResponse>> ResendVerificationEmailAsync(string email);
    
    /// <summary>
    /// Initiate password reset process
    /// FR-3: Password Reset via Email
    /// </summary>
    Task<Result<PasswordResetResponse>> ForgotPasswordAsync(string email);
    
    /// <summary>
    /// Reset password using token
    /// FR-3.5: Validate token before allowing password change
    /// </summary>
    Task<Result<PasswordResetResponse>> ResetPasswordAsync(ResetPasswordRequest request);
    
    /// <summary>
    /// Logout user (revoke refresh token)
    /// </summary>
    Task LogoutAsync(int userId, string refreshToken);
    
    /// <summary>
    /// Logout from all devices (revoke all refresh tokens)
    /// </summary>
    Task LogoutAllDevicesAsync(int userId);
}
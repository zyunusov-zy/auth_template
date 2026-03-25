namespace AuthSystemTemplate.Application.DTOs.Auth;

public record EmailVerificationResponse(
    bool Success,
    string Message
);
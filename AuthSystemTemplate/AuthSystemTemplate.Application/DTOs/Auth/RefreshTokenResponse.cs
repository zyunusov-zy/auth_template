namespace AuthSystemTemplate.Application.DTOs.Auth;

public record RefreshTokenResponse(
    string AccessToken,
    int ExpiresIn
);
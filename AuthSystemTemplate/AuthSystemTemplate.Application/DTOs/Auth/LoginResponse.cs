using AuthSystemTemplate.Application.DTOs.User;

namespace AuthSystemTemplate.Application.DTOs.Auth;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string TokenType,
    UserDto User
);
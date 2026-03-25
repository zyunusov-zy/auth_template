namespace AuthSystemTemplate.Application.DTOs.User;

public record UserDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles,
    bool EmailVerified,
    DateTime CreatedAt
);
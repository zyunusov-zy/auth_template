namespace AuthSystemTemplate.Application.DTOs.User;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);
using AuthSystemTemplate.Application.Common.Results;
using AuthSystemTemplate.Application.DTOs.Auth;
using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AuthSystemTemplate.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passHash;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passHash, ITokenService tokenService,
        IEmailService emailService, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _passHash = passHash;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        var now = DateTime.UtcNow;
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email);
            if (emailExists)
                return Result<RegisterResponse>.Failure(Error.Validation("Email already exists"));

            var (hash, salt) = _passHash.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                FirstName = request.FirstName,
                CreatedAt = now
            };

            var defaultRole = await _unitOfWork.Roles.GetByNameAsync(Roles.User);
            if (defaultRole == null)
            {
                return Result<RegisterResponse>.Failure(Error.NotFound("Role was not found"));
            }

            var userRole = new UserRole
            {
                User = user,
                RoleId = defaultRole.Id,
                AssignedAt = now
            };

            user.UserRoles.Add(userRole);

            var verificationToken = _tokenService.GenerateEmailVerificationToken();
            var emailToken = new EmailVerificationToken
            {
                Token = verificationToken,
                ExpiresAt = now.AddHours(24),
                CreatedAt = now,
                IsUsed = false,
                User = user
            };

            user.EmailVerificationTokens.Add(emailToken);

            await _unitOfWork.Users.AddAsync(user);

            await _emailService.SendEmailVerificationAsync(
                user.Email,
                verificationToken,
                user.FirstName
            );

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();


            return Result<RegisterResponse>.Success(new RegisterResponse(user.Id,
                "Registration successful. Please check your email."));
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(e, "Registration failed for {Email}", request.Email);
            throw;
        }
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<EmailVerificationResponse> VerifyEmailAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<EmailVerificationResponse> ResendVerificationEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordResetResponse> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordResetResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(int userId, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAllDevicesAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
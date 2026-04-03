using AuthSystemTemplate.Application.Common.Results;
using AuthSystemTemplate.Application.DTOs.Auth;
using AuthSystemTemplate.Application.DTOs.User;
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

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<LoginResponse>.Failure(Error.Unauthorized("Invalid credentials"));
            }

            var isValidPass = _passHash.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValidPass)
            {
                return Result<LoginResponse>.Failure(Error.Unauthorized("Invalid credentials"));
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name.ToString());
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var rolesList = user.UserRoles.Select(ur => ur.Role.Name.ToString()).ToList();

            var userDto = new UserDto
            (
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                rolesList,
                user.EmailVerified,
                user.CreatedAt
            );
            return Result<LoginResponse>.Success(new LoginResponse(
                accessToken,
                refreshToken,
                3600, // need to change it 
                "Bearer",
                userDto));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Login failed for {Email}", request.Email);
            throw;
        }
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var refreshTokenDb = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            if (refreshTokenDb == null
                || refreshTokenDb.IsRevoked
                || refreshTokenDb.ExpiresAt < DateTime.UtcNow)
                return Result<RefreshTokenResponse>.Failure(Error.Unauthorized("Invalid token"));
            var user = refreshTokenDb.User;
            var roles = user.UserRoles.Select(ur => ur.Role.Name.ToString());
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse(accessToken, 3600));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Refreshing token failed for token: {RefreshToken}", refreshToken);
            throw;
        }
    }

    public async Task<Result<EmailVerificationResponse>> VerifyEmailAsync(string token)
    {
        try
        {
            var tokenDb = await _unitOfWork.EmailVerificationTokens.FirstOrDefaultAsync(x => x.Token == token.Trim());
            if (tokenDb is null)
                return Result<EmailVerificationResponse>.Failure(Error.NotFound("Not valid token"));
            if (tokenDb.ExpiresAt < DateTime.UtcNow)
                return Result<EmailVerificationResponse>.Failure(Error.Unauthorized("Token expired"));
            if (tokenDb.IsUsed)
                return Result<EmailVerificationResponse>.Failure(Error.Unauthorized("Token already used"));
            tokenDb.IsUsed = true;
            await _unitOfWork.EmailVerificationTokens.UpdateAsync(tokenDb);
            var user = await _unitOfWork.Users.GetByIdAsync(tokenDb.UserId);
            if (user is null)
                return Result<EmailVerificationResponse>.Failure(Error.NotFound("User not found"));
            user.EmailVerified = true;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            await _emailService.SendWelcomeEmailAsync(user);
            return Result<EmailVerificationResponse>.Success(
                new EmailVerificationResponse(true, "Successfully verified email"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<EmailVerificationResponse>> ResendVerificationEmailAsync(string email)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email.Trim());
            if (user is null)
                return Result<EmailVerificationResponse>.Failure(Error.NotFound("User not found"));
            if (user.EmailVerified)
                return Result<EmailVerificationResponse>.Failure(Error.Conflict("Email already verified"));
            var verTokens = await _unitOfWork.EmailVerificationTokens.FindAsync(x => x.UserId == user.Id);
            foreach (var token in verTokens)
                token.IsUsed = true;

            var now = DateTime.UtcNow;

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
            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailVerificationAsync(
                user.Email,
                verificationToken,
                user.FirstName
            );

            return Result<EmailVerificationResponse>.Success(
                new EmailVerificationResponse(true, "Verification email resent"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<PasswordResetResponse>> ForgotPasswordAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user is null)
            return Result<PasswordResetResponse>.Failure(Error.NotFound("User not found with email"));
        await _unitOfWork.PasswordResetTokens.InvalidateUserTokensAsync(user.Id);

        var now = DateTime.UtcNow;
        var token = _tokenService.GeneratePasswordResetToken();
        var passwordResetToken = new PasswordResetToken
        {
            Token = token,
            CreatedAt = now,
            ExpiresAt = now.AddHours(1),
            IsUsed = false,
            UserId = user.Id,
            UsedAt = null,
            User = user
        };

        await _unitOfWork.PasswordResetTokens.AddAsync(passwordResetToken);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendPasswordResetEmailAsync(user.Email, token, user.FirstName);

        return Result<PasswordResetResponse>.Success(new PasswordResetResponse(true, "A reset link has been sent"));
    }

    public async Task<Result<PasswordResetResponse>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var token = await _unitOfWork.PasswordResetTokens.GetByTokenAsync(request.Token);
        if (token is null)
            return Result<PasswordResetResponse>.Failure(Error.NotFound("Token not found"));
        if (token.IsUsed || token.ExpiresAt < DateTime.UtcNow)
            return Result<PasswordResetResponse>.Failure(Error.Failure("Token is not valid"));
        var user = token.User;
        var isSamePassword = _passHash.VerifyPassword(request.NewPassword, user.PasswordHash, user.PasswordSalt);
        if (isSamePassword)
            return Result<PasswordResetResponse>.Failure(
                Error.Conflict("New password cannot be the same as the old password"));
        var (hash, salt) = _passHash.HashPassword(request.NewPassword);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        token.IsUsed = true;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.PasswordResetTokens.UpdateAsync(token);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendPasswordChangedConfirmationAsync(user.Email, user.FirstName);
        return Result<PasswordResetResponse>.Success(new PasswordResetResponse(true, "Password reset successfully"));
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
using AuthSystemTemplate.Application.DTOs.Auth;
using FluentValidation;

namespace AuthSystemTemplate.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().WithMessage("Email field required").EmailAddress();
        RuleFor(r => r.Password).NotEmpty().MinimumLength(8)
            .Matches("[A-Z]+").WithMessage("'{PropertyName}' must contain one or more capital letters.")
            .Matches("[a-z]+").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
            .Matches(@"(\d)+").WithMessage("'{PropertyName}' must contain one or more digits.")
            .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]")
            .WithMessage("'{ PropertyName}' must contain one or more special characters.")
            .Matches("(?!.*[£# “”])")
            .WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");
        RuleFor(r => r.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(r => r.LastName).NotEmpty().MinimumLength(2);
    }
}
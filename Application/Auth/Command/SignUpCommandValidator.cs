using FluentValidation;

namespace Application.Auth.Command;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    { 
    }
}

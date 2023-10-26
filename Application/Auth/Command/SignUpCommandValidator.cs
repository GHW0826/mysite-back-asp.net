using FluentValidation;

namespace Application.Auth.Command;

public class SignInQueryValidator : AbstractValidator<SignUpCommand>
{
    public SignInQueryValidator()
    { 
    }
}

using FluentValidation;

namespace Application.User.Command;

public class SignInQueryValidator : AbstractValidator<SignUpCommand>
{
    public SignInQueryValidator()
    {
    }
}

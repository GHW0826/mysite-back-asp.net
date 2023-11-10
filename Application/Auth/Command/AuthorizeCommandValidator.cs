using FluentValidation;

namespace Application.Auth.Command;

public class AuthorizeCommandValidator : AbstractValidator<AuthorizeCommand>
{
    public AuthorizeCommandValidator()
    {
    }
}
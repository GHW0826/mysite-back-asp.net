using FluentValidation;

namespace Application.Auth.Query;

public class SignInQueryValidator : AbstractValidator<SignInQuery>
{
    public SignInQueryValidator()
    { 
    }
}

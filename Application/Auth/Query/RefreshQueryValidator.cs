using FluentValidation;

namespace Application.Auth.Query;

public class RefreshQueryValidator : AbstractValidator<RefreshQuery>
{
    public RefreshQueryValidator()
    {
    }
}

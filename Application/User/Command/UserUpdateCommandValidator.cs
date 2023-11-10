using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.User.Command;

public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
    }
}
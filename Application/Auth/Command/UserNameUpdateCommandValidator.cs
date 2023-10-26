using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Auth.Command;

public class UserNameUpdateCommandValidator : AbstractValidator<UserNameUpdateCommand>
{
    public UserNameUpdateCommandValidator()
    {

        //RuleFor(p => p.ParamX)
        //    .CorrectPid()//공통 Validation. 위치 Common/Validator      
    }

    //사용자 정의 유효성 체크 
    public bool BeCorrectPrefixCn(string cn)
    {
        return cn.StartsWith("WJ");//윈조이 CN은 접두어 WJ를 사용
    }
}
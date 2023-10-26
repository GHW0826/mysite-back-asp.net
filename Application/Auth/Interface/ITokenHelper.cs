using Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Common.Interfaces;

namespace Application.Auth.Interface;

public interface ITokenHelper
{
    public string GenerateJWTToken(AuthUser userInfo);
}

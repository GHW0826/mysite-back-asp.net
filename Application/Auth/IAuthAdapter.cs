using Application.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth;

public interface IAuthAdapter
{
    Task<SignUpModel> SignUp(string email, string password);

    Task<SignInModel> SignIn(string email, string password);
}

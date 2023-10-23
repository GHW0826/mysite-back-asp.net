using Application.Auth;
using Application.Auth.Interface;
using Application.Auth.Model;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Common.Exceptions;

namespace Infrastructure.adapter;

public class AuthAdapter : IAuthAdapter
{
    readonly ILogger<AuthAdapter> _logger;
    readonly ITokenHelper _tokenHelper;
    readonly AuthContext _context;

    public AuthAdapter(ILogger<AuthAdapter> logger, AuthContext context, ITokenHelper tokenHelper)
    {
        _logger = logger;
        _tokenHelper = tokenHelper;
        _context = context;
    }

    public async Task<SignInModel> SignIn(string email, string password)
    {
        var result = await _context.signUpEntities.Where(b => b.email == email)
            .Where(b => b.password == password).FirstOrDefaultAsync();
        return new SignInModel
        {
            email = result.email
        };
    }

    public async Task<SignUpModel> SignUp(string email, string password)
    {
        var result = await _context.findByEmailAndPassword(email, password)
                            ?? throw new AuthorizationException("invalid user");

        return new SignUpModel
        {
            email = result.email,
            password = result.password
        };
    }
}
